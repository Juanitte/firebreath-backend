﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyWeb.TicketsMicroservice.Translations {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Translation_Tickets {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Translation_Tickets() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tickets.TicketsMicroservice.Translations.Translation_Tickets", typeof(Translation_Tickets).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
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
        ///   Busca una cadena traducida similar a Hi, dear customer. You can keep tracking of your ticket and communicate with a technician using the following link: .
        /// </summary>
        public static string Email_body {
            get {
                return ResourceManager.GetString("Email_body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a IoT ticket tracking.
        /// </summary>
        public static string Email_title {
            get {
                return ResourceManager.GetString("Email_title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Couldn&apos;t create ticket.
        /// </summary>
        public static string Error_create_ticket {
            get {
                return ResourceManager.GetString("Error_create_ticket", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The {0} field couldn&apos;t be filtered by {1}.
        /// </summary>
        public static string Error_filter {
            get {
                return ResourceManager.GetString("Error_filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Couldn&apos;t obtain the filtered tickets.
        /// </summary>
        public static string Error_ticket_filter {
            get {
                return ResourceManager.GetString("Error_ticket_filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Error updating tickets.
        /// </summary>
        public static string Error_update {
            get {
                return ResourceManager.GetString("Error_update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The ticket is already assigned to a technician.
        /// </summary>
        public static string Ticket_already_assigned {
            get {
                return ResourceManager.GetString("Ticket_already_assigned", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The ticket has no messages.
        /// </summary>
        public static string Ticket_no_messages {
            get {
                return ResourceManager.GetString("Ticket_no_messages", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ticket not found.
        /// </summary>
        public static string Ticket_not_found {
            get {
                return ResourceManager.GetString("Ticket_not_found", resourceCulture);
            }
        }
    }
}
