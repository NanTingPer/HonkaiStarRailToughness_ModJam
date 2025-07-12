using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace ModJam.Nets;

[AttributeUsage(AttributeTargets.Field)]
public class NetFieldAttribute : Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class NetPropAttribute : Attribute;
public static class NPCAutoNet
{
    private readonly static Dictionary<Type, List<FieldInfo>> s_netFieldInfo = [];
    private readonly static Dictionary<Type, List<PropertyInfo>> s_netPropInfo = [];

    [ModuleInitializer]
    internal static void InitNPCAutoNet()
    {
        var gtypes = AssemblyManager.GetLoadableTypes(typeof(NPCAutoNet).Assembly).Where(f => f.IsSubclassOf(typeof(GlobalNPC)));
        foreach (var type in gtypes) {
            foreach (var field in type.GetFields()) {
                if(field.GetCustomAttribute<NetFieldAttribute>() != null) {
                    AddToDictionary(field, s_netFieldInfo);
                }
            }

            foreach (var propertyInfo in type.GetProperties()) {
                if (propertyInfo.GetCustomAttribute<NetPropAttribute>() != null) {
                    AddToDictionary(propertyInfo, s_netPropInfo);
                }
            }
        }
    }

    public static void AddToDictionary(MemberInfo info, IDictionary dictionary)
    {
        var type = info.DeclaringType;
        if (type == null)
            return;

        if (dictionary.Contains(type)) {
            ((IList)dictionary[type]).Add(info);
        } else {
            if(info.MemberType == MemberTypes.Field) {
                var list = new List<FieldInfo>()
                {
                    info as FieldInfo
                };
                dictionary[type] = list;
            }

            if(info.MemberType == MemberTypes.Property) {
                var list = new List<PropertyInfo>()
                {
                    info as PropertyInfo
                };
                dictionary[type] = list;
            }
        }

    }
}
