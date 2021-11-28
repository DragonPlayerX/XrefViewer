using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using UnhollowerRuntimeLib.XrefScans;

namespace XrefViewer.Core
{
    public static class XrefCore
    {
        private static XrefViewerWindow Window => XrefViewerMod.Instance.ViewerWindow;

        private static readonly List<Type> BlacklistedTypes = new List<Type>() { typeof(Il2CppSystem.Object), typeof(object) };

        public static void Scan(string typeName, string methodName, bool printStrings, bool exactName)
        {
            if (typeName == null)
            {
                Window.WriteLine(Color.Red, "ERROR: No type defined");
                return;
            }

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
            {
                Window.WriteLine(Color.Red, "ERROR: Type not found");
                return;
            }

            if (methodName != null)
            {
                if (exactName)
                {
                    DumpMethod(type.GetMethod(methodName), printStrings);
                }
                else
                {
                    try
                    {
                        MethodInfo[] methods = type.GetMethods();

                        foreach (MethodInfo method in methods)
                        {
                            if (method.Name.Contains(methodName))
                                DumpMethod(method, printStrings);
                        }
                    }
                    catch (Exception e)
                    {
                        Window.WriteLine(Color.Red, "An error occurred: " + e.ToString());
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
                            DumpMethod(method, printStrings);
                    }
                }
                catch (Exception e)
                {
                    Window.WriteLine(Color.Red, "An error occurred: " + e.ToString());
                }
            }
        }

        private static void DumpMethod(MethodInfo method, bool printStrings)
        {
            try
            {
                if (method == null)
                {
                    Window.WriteLine(Color.Red, "ERROR: Method not found");
                    return;
                }

                Window.WriteLine(Color.Yellow, "Scanning " + method.Name + "...");

                if (printStrings)
                {
                    Window.WriteLine(Color.LightGreen, "Method strings: ");
                    DumpXrefMethodString(XrefScanner.XrefScan(method));
                }
                else
                {
                    Window.WriteLine(Color.LightGreen, "Method is using: ");
                    DumpXrefMethod(XrefScanner.XrefScan(method));

                    Window.WriteLine(Color.LightGreen, "Method is used by:");
                    DumpXrefMethod(XrefScanner.UsedBy(method));
                }
            }
            catch (Exception)
            {
                Window.WriteLine(Color.Red, "ERROR: Method cannot be scanned");
            }
        }

        private static void DumpXrefMethodString(IEnumerable<XrefInstance> instances)
        {
            foreach (XrefInstance instance in instances)
            {
                if (instance.Type == XrefType.Global)
                    Window.WriteLine(Color.White, "String: " + instance.ReadAsObject().ToString());
            }
        }

        private static void DumpXrefMethod(IEnumerable<XrefInstance> instances)
        {
            int methods = 0;
            int resolvedMethods = 0;
            foreach (XrefInstance instance in instances)
            {
                if (instance.Type == XrefType.Global)
                {
                    Window.WriteLine(Color.White, "String: " + instance.ReadAsObject().ToString() + "\n");
                }
                else
                {
                    MethodBase resolvedMethod = instance.TryResolve();
                    if (resolvedMethod != null)
                    {
                        Window.WriteLine(Color.White, "Type: " + resolvedMethod.DeclaringType.FullName + "\n" +
                            "Method: " + resolvedMethod.Name + "\n");

                        resolvedMethods++;
                    }
                }
                methods++;
                Application.DoEvents();
            }

            Window.WriteLine(Color.LightBlue, methods + " methods, " + resolvedMethods + " resolved methods\n");
        }
    }
}
