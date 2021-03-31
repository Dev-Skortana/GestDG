using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Gest.Interface_SQLiteAccess
{
    public class Manager_path_database
    {
        public const String NAME_FOLDER_DATABASE = "Databases";
        public const String NAME_FILE_DATABASE = "Gest";
        public const String TYPE_FILE_DATABASE = "db";

        private String Path_root_application { get; set; }

        public Manager_path_database(String path_root_application)
        {
            this.Path_root_application = path_root_application;
        }

        
        private String get_path_folder_database() => Path.Combine(this.Path_root_application, NAME_FOLDER_DATABASE);

        public String get_path_file_database() => Path.Combine(this.Path_root_application,NAME_FOLDER_DATABASE,this.get_name_file_database_full());
        public String get_name_file_database_full() => $"{NAME_FILE_DATABASE}.{TYPE_FILE_DATABASE}";
        
        public void create_folder_database_ifnotexist()
        {
            if (!Directory.Exists(this.get_path_folder_database()))
            {
                Directory.CreateDirectory(this.get_path_folder_database());
            }
        }
        
    }
}
