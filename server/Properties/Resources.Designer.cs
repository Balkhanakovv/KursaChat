﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace server.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("server.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Поиск локализованного ресурса типа System.Byte[].
        /// </summary>
        public static byte[] KursaChat {
            get {
                object obj = ResourceManager.GetObject("KursaChat", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 25.06.2019 9:43:07	1: qwwq
        ///25.06.2019 9:43:09	vov4ik: Привет!
        ///25.06.2019 9:43:24	vov4ik: Как дела?
        ///25.06.2019 9:43:35	vov4ik: Поставте 5, пожалуйста
        ///25.06.2019 9:44:02	1: 134 character succ
        ///25.06.2019 9:44:05	vov4ik: &gt;_&lt;
        ///25.06.2019 9:51:55	11: dfdfd
        ///25.06.2019 10:08:58	user2: rtde5te
        ///25.06.2019 10:09:08	user: Поставьте 5
        ///25.06.2019 10:09:20	user: плс
        ///25.06.2019 10:09:28	user2: test
        ///25.06.2019 10:09:49	user: :)
        ///25.06.2019 10:11:03	user2: 111
        ///25.06.2019 10:11:03	user2: 
        ///25.06.2019 10:11:20	user2 [остаток строки не уместился]&quot;;.
        /// </summary>
        public static string logList {
            get {
                return ResourceManager.GetString("logList", resourceCulture);
            }
        }
    }
}
