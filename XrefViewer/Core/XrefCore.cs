using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib.XrefScans;

using XrefViewer.Core.Network;

namespace XrefViewer.Core
{
    public static class XrefCore
    {
        private static readonly List<Type> BlacklistedTypes = new List<Type>() { typeof(Il2CppSystem.Object), typeof(object) };

        public static string Scan(string typeName, string methodName, bool exactName, ref List<XrefObject> xrefResult)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrWhiteSpace(typeName))
                return "No type defined";

            Type type = null;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = assembly.GetType(typeName);
                if (t != null)
                {
                    type = t;
                    break;
                }
            }

            if (type == null)
                return "Type not found";

            if (methodName != null)
            {
                if (exactName)
                {
                    ScanMethod(type.GetMethod(methodName), ref xrefResult);
                }
                else
                {
                    try
                    {
                        MethodInfo[] methods = type.GetMethods();

                        bool foundMethod = false;
                        foreach (MethodInfo method in methods)
                        {
                            if (method.Name.Contains(methodName))
                            {
                                ScanMethod(method, ref xrefResult);
                                foundMethod = true;
                            }
                        }

                        if (!foundMethod)
                            return "Method not found";
                    }
                    catch (Exception e)
                    {
                        return "An error occurred: " + e.ToString();
                    }
                }
            }
            else
            {
                try
                {
                    MethodInfo[] methods = type.GetMethods();

                    foreach (MethodInfo method in methods)
                    {
                        if (!BlacklistedTypes.Contains(method.DeclaringType))
                            ScanMethod(method, ref xrefResult);
                    }
                }
                catch (Exception e)
                {
                    return "An error occurred: " + e.ToString();
                }
            }

            return null;
        }

        private static string ScanMethod(MethodInfo method, ref List<XrefObject> xrefResult)
        {
            try
            {
                if (method == null)
                    return "Method not found";

                XrefObject xrefObject = new XrefObject();
                xrefObject.Type = XrefObject.XrefType.Method;
                xrefObject.Name = method.Name;
                xrefObject.MethodType = method.DeclaringType.FullName;
                xrefObject.Pointer = 0;

                XrefInstance[] usingResult = XrefScanner.XrefScan(method).ToArray();
                if (usingResult.Length < 512)
                    xrefObject.IsUsing.AddRange(ScanXrefInstances(usingResult));

                XrefInstance[] usedByResult = XrefScanner.UsedBy(method).ToArray();
                if (usedByResult.Length < 512)
                    xrefObject.UsedBy.AddRange(ScanXrefInstances(usedByResult));

                xrefResult.Add(xrefObject);
            }
            catch (Exception)
            {
                return "Unable to scan method";
            }

            return null;
        }

        private static List<XrefObject> ScanXrefInstances(IEnumerable<XrefInstance> instances)
        {
            List<XrefObject> xrefResult = new List<XrefObject>(); ;

            foreach (XrefInstance instance in instances)
            {
                XrefObject xrefObject = new XrefObject();
                xrefObject.Type = XrefObject.XrefType.Unresolved;

                if (instance.Type == XrefType.Global)
                {
                    xrefObject.Name = instance.ReadAsObject().ToString();
                    xrefObject.Type = XrefObject.XrefType.String;
                    xrefObject.Pointer = instance.Pointer.ToInt64();
                }
                else
                {
                    MethodBase resolvedMethod = instance.TryResolve();
                    if (resolvedMethod != null)
                    {
                        xrefObject.Name = resolvedMethod.Name;
                        xrefObject.MethodType = resolvedMethod.DeclaringType.FullName;
                        xrefObject.Type = XrefObject.XrefType.Method;
                        xrefObject.Pointer = instance.Pointer.ToInt64();
                    }
                }

                xrefResult.Add(xrefObject);
            }

            return xrefResult;
        }
    }
}
