#pragma warning disable CA2255
using HonkaiStarRailToughness.Toughnesss;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;

namespace HonkaiStarRailToughness.Nets;

[AttributeUsage(AttributeTargets.Field)]
public class NetFieldAttribute : Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class NetPropAttribute : Attribute;
public static class NPCAutoNet
{
    private readonly static Dictionary<Type, List<FieldInfo>> s_netFieldInfo = [];
    private readonly static Dictionary<Type, List<PropertyInfo>> s_netPropInfo = [];
    private readonly static Dictionary<Type, MethodInfo> ReadMethod = [];
    private readonly static Dictionary<Type, MethodInfo> WriteMethod = [];

    [ModuleInitializer]
    internal static void RegionStreamMethod()
    {
        var writeMethods = typeof(BinaryWriter).GetMethods().Where(m => m.Name == "Write" && m.GetParameters().Length == 1);
        foreach (var methodInfo in writeMethods) {
            var type = methodInfo.GetParameters()[0].ParameterType;
            if(!WriteMethod.TryGetValue(type, out _)) {
                WriteMethod.Add(type, methodInfo);
            }
        }

        var readMethods = typeof(BinaryReader).GetMethods()
            .Where(m =>
                m.Name.StartsWith("Read") &&
                m.Name != "Read" &&
                m.Name != "Read7BitEncodedInt" &&
                m.Name != "Read7BitEncodedInt64" &&
                m.GetParameters().Length == 0
            );

        foreach (var method in readMethods) {
            var type = method.ReturnParameter.ParameterType;
            if (!ReadMethod.TryGetValue(type, out _)) {
                ReadMethod.Add(type, method);
            }
        }

        #region Utils
        var utype = typeof(Utils);
        var readVector2 = utype.GetMethod("ReadVector2", BindingFlags.Static | BindingFlags.Public);
        var readRGB = utype.GetMethod("ReadRGB", BindingFlags.Static | BindingFlags.Public);
        ReadMethod[typeof(Vector2)] = readVector2;
        ReadMethod[typeof(Color)] = readRGB;

        var writeRGB = utype.GetMethod("WriteRGB", BindingFlags.Static | BindingFlags.Public);
        var writeVector2 = utype.GetMethod("WriteVector2", BindingFlags.Static | BindingFlags.Public);
        WriteMethod[typeof(Vector2)] = writeVector2;
        WriteMethod[typeof(Color)] = writeRGB;
        #endregion

    }

    private static List<Hook> hks = [];
    [ModuleInitializer]
    internal static void InitNPCAutoNet()
    {
        var gtypes = typeof(HonkaiStarRailToughness).Assembly.GetTypes()
            .Where(f => f.IsSubclassOf(typeof(GlobalNPC)) &&
                f.IsAbstract == false
            );
        foreach (var type in gtypes) {
            s_netFieldInfo[type] = [];
            s_netPropInfo[type] = [];
            foreach (var field in type.GetFields()) {
                if(field.GetCustomAttribute<NetFieldAttribute>() != null) {
                    s_netFieldInfo[type].Add(field);
                }
            }
    
            foreach (var propertyInfo in type.GetProperties()) {
                if (propertyInfo.GetCustomAttribute<NetPropAttribute>() != null) {
                    s_netPropInfo[type].Add(propertyInfo);
                }
            }
        }
    
        foreach (var keyValue in s_netFieldInfo) {
            var k = keyValue.Value.ToHashSet().ToList();
            keyValue.Value.Clear();
            keyValue.Value.AddRange(k);
            keyValue.Value.Sort((a, b) => a.MetadataToken.CompareTo(b.MetadataToken));
        }
    
        foreach (var keyValue in s_netPropInfo) {
            var k = keyValue.Value.ToHashSet().ToList();
            keyValue.Value.Clear();
            keyValue.Value.AddRange(k);
            keyValue.Value.Sort((a, b) => a.MetadataToken.CompareTo(b.MetadataToken));
        }
    
        foreach (var netType in gtypes) {
            if (s_netFieldInfo[netType].Count != 0 || s_netPropInfo[netType].Count != 0) {
                var sendMethod = netType.GetMethod("SendExtraAI");
                var receiveMethod = netType.GetMethod("ReceiveExtraAI");

                hks.Add(new Hook(sendMethod, SendExtraAIHook));
                hks.Add(new Hook(receiveMethod, ReceiveExtraAIHook));
            }
            //MonoModHooks.Add(sendMethod, SendExtraAIHook);
            //MonoModHooks.Add(receiveMethod, ReceiveExtraAIHook);
        }
    }

    internal delegate void SendExtraAI(object gnpc, NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter);
    internal static void SendExtraAIHook(SendExtraAI orig, object gnpc, NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        if (s_netFieldInfo.TryGetValue(gnpc.GetType(), out var fields)) {
            foreach (var fieldInfo in fields) {
                try {
                    binaryWriter.WriteField(fieldInfo, fieldInfo.GetValue(gnpc));
                    var size = binaryWriter.BaseStream.Position;
                    //Logging.PublicLogger.Debug($"发送数据包: {size}字节");
                } catch(Exception e) {
                    //Logging.PublicLogger.Error(e.Message + e.StackTrace);
                }
            }
        }
        if (s_netPropInfo.TryGetValue(gnpc.GetType(), out var props)) {
            foreach (var propInfo in props) {
                binaryWriter.WriteProp(propInfo, propInfo.GetValue(gnpc));
            }
        }
        orig.Invoke(gnpc, npc, bitWriter, binaryWriter);
    }

    internal delegate void ReceiveExtraAI(object gnpc, NPC npc, BitReader bitReader, BinaryReader binaryReader);
    internal static void ReceiveExtraAIHook(ReceiveExtraAI orig, object gnpc, NPC npc, BitReader bitReader, BinaryReader binaryReader)
    {
        //binaryReader.BaseStream.Position = 0;
        var gtype = gnpc.GetType();
        if (s_netFieldInfo.TryGetValue(gtype, out var fields)) {
            foreach (var field in fields) {
                _ = binaryReader.ReadField(field, gnpc);
            }
        }
        if (s_netPropInfo.TryGetValue(gtype, out var props)) {
            foreach (var propinfo in props) {
                binaryReader.ReadProp(propinfo, gnpc);
            }
        }
        orig.Invoke(gnpc, npc, bitReader, binaryReader);
    }

    public static object ReadField(this BinaryReader br, FieldInfo field, object tarObj)
    {
        var methodInfo = ReadMethod[field.FieldType];
        object value = null;
        if (methodInfo.GetParameters().Length == 1) {
            value = methodInfo.Invoke(null, [br]);
            //Logging.PublicLogger.Info("流位置: " + br.BaseStream.Position + "  读字段 :  " + field.Name + "   Value : " + value + "  MethodInfo: " + methodInfo.ReturnParameter.ParameterType.FullName);
        } else {
            value = methodInfo.Invoke(br, []);
            //Logging.PublicLogger.Info("流位置: " + br.BaseStream.Position + "  读字段 :  " + field.Name + "   Value : " + value + "  MethodInfo: " + methodInfo.ReturnParameter.ParameterType.FullName);
        }
        field.SetValue(tarObj, value);
        return value;
    }

    public static object ReadProp(this BinaryReader br, PropertyInfo prop, object tarObj)
    {
        if (br.BaseStream.Position >= br.BaseStream.Length)
            return null;
        var methodInfo = ReadMethod[prop.PropertyType];
        object value = null;
        if (methodInfo.GetParameters().Length == 1) {
            value = methodInfo.Invoke(null, [br]);
        } else {
            value = methodInfo.Invoke(br, []);
        }
        prop.SetValue(tarObj, value);
        return value;
    }

    public static void WriteField(this BinaryWriter bw, FieldInfo field, object value)
    {
        var methodinfo = WriteMethod[field.FieldType];
        if(methodinfo.GetParameters().Length == 2) {
            methodinfo.Invoke(null, [bw, value]);
            //Logging.PublicLogger.Info("写字段 :  " + field.Name + "   Value : " + value + "  MethodInfo: " + methodinfo.GetParameters()[1].ParameterType.FullName);
            return;
        }
        methodinfo.Invoke(bw, [value]);
        //Logging.PublicLogger.Info("写字段 :  " + field.Name + "   Value : " + value + "  MethodInfo: " + methodinfo.GetParameters()[0].ParameterType.FullName);
    }

    public static void WriteProp(this BinaryWriter bw, PropertyInfo prop, object value)
    {
        var methodinfo = WriteMethod[prop.PropertyType];
        if (methodinfo.GetParameters().Length == 2) {
            methodinfo.Invoke(null, [bw, value]);
            return;
        }
        methodinfo.Invoke(bw, [value]);
    }
}
