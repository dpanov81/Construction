using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

namespace Construction.FileServise
{
    class FileIO
    {
        string Path = "data.txt";               // Файл с данными создается рядом с exe-шником

        /// <summary>
        /// Загрузка данных в JSON формате из файла в BindingList.
        /// </summary>
        /// <returns>BindingList</returns>
        public BindingList<Material> LoadData()
        {
            bool fileExists = File.Exists(Path);
            if (!fileExists)
            {
                File.CreateText(Path).Dispose();
                return new BindingList<Material>();
            }
            using (var reader = File.OpenText(Path))
            {
                string fileText = reader.ReadToEnd();

                if(fileText == "")
                    return new BindingList<Material>();

                return JsonConvert.DeserializeObject<BindingList<Material>>(fileText);
            }
        }

        /// <summary>
        /// Сохранение данных из коллекции BindingList в файл, в JSON формате.
        /// </summary>
        public void SaveData(BindingList<Material> c)
        {
            using (StreamWriter writer = File.CreateText(Path))
            {
                string output = JsonConvert.SerializeObject(c);
                writer.Write(output);
            }
        }
    }
}
