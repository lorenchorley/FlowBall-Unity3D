using System;
using System.Collections.Generic;
using NoFlo_Basic;

namespace NoFloEditor {

    public static class ComponentCatalog {

        private static Dictionary<string, List<Type>> ComponentsByPackage;
        private static Dictionary<string, Type> ComponentsByQualifiedName;
        private static Dictionary<Type, string> QualifedNameByComponentType;
        private static bool isInitialised = false;

        public static Dictionary<string, List<Type>> RequestComponentsByPackage() {
            if (!isInitialised)
                CatalogAvailableComponents();

            return ComponentsByPackage;
        }

        public static Dictionary<string, Type> RequestComponentsByQualifiedName() {
            if (!isInitialised)
                CatalogAvailableComponents();

            return ComponentsByQualifiedName;
        }

        public static Dictionary<Type, string> RequestQualifedNameByComponentType() {
            if (!isInitialised)
                CatalogAvailableComponents();

            return QualifedNameByComponentType;
        }

        private static void CatalogAvailableComponents() {
            ComponentsByPackage = new Dictionary<string, List<Type>>();
            ComponentsByQualifiedName = new Dictionary<string, Type>();
            QualifedNameByComponentType = new Dictionary<Type, string>();

            List<Type> packageList;
            foreach (Type t in ReflectiveEnumerator.GetEnumerableOfType<NoFlo_Basic.Component>()) {

                string name = t.GetAttributeValue((ComponentNameAttribute dna) => dna.name);
                string package = t.GetAttributeValue((ComponentPackageAttribute dna) => dna.name);
                string qualifiedName = package + "/" + name;

                if (name == "")
                    throw new Exception("Component " + t.Name + " does not have valid attribute ComponentName");

                if (package == "")
                    throw new Exception("Component " + t.Name + " does not have valid attribute ComponentPackage");

                if (ComponentsByQualifiedName.ContainsKey(qualifiedName))
                    throw new Exception("TODO");

                ComponentsByQualifiedName.Add(qualifiedName, t);

                if (!ComponentsByPackage.TryGetValue(package, out packageList)) {
                    packageList = new List<Type>();
                    ComponentsByPackage.Add(package, packageList);
                }

                if (packageList.Contains(t))
                    throw new Exception("TODO");

                packageList.Add(t);

                QualifedNameByComponentType.Add(t, qualifiedName);

            }

            isInitialised = true;
        }

    }

}