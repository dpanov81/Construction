using System;
using System.ComponentModel;
using System.Windows.Forms;
using Construction;
using Construction.FileServise;

namespace Сonstruction
{
    public partial class Form1 : Form
    {
        private BindingList<Material> ListMaterials;

        public Form1()
        {
            InitializeComponent();
        }         
        private void Form1_Load(object sender, EventArgs e)
        {            
            FileIO file = new FileIO();
            
            try
            {
                ListMaterials = file.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }

            LoadList();
        }

        /// <summary>
        /// Загрузка данных в список.
        /// </summary>
        private void LoadList()
        {
            listView1.Items.Clear();
            int count = ListMaterials.Count;

            decimal TotalAmount = 0;                    // Общая сумма

            for (int i = 0; i < count; i++)
            {
                ListViewItem newItem = listView1.Items.Add(ListMaterials[i].Date);
                newItem.SubItems.Add(ListMaterials[i].Materials);
                newItem.SubItems.Add(ListMaterials[i].Cost.ToString());

                TotalAmount += ListMaterials[i].Cost;
            }

            labelCount.Text = TotalAmount.ToString();
        }
        /// <summary>
        /// Добавление материала в список.
        /// </summary>        
        private void AddButton_Click(object sender, EventArgs e)
        {
            AddMaterialForm materialForm = new AddMaterialForm();

            if (materialForm.ShowDialog() != DialogResult.OK)
                return;

            if (materialForm.textBoxMaterial.Text != "" && materialForm.textBoxCost.Text != "")
            {
                decimal cost;

                if (decimal.TryParse(materialForm.textBoxCost.Text, out cost))
                {
                    Material material = new Material();
                    material.SetData(materialForm.textBoxMaterial.Text, cost);
                    ListMaterials.Add(material);

                    ListViewItem newItem = listView1.Items.Add(material.Date);
                    newItem.SubItems.Add(material.Materials);                          
                    newItem.SubItems.Add(material.Cost.ToString());                  
                    
                    FileIO file = new FileIO();
                    file.SaveData(ListMaterials);
                    
                    LoadList();
                }
                else
                    MessageBox.Show("Некорректный ввод данных");
            }
        }

        /// <summary>
        /// Удаление элемента из списка.
        /// </summary>
        private void DelButton_Click(object sender, EventArgs e)
        {           
            if (listView1.SelectedItems.Count == 0)       // Если Item не выбран - возвращаяемся
                return;

            const string message = "Вы уверенны что хотите удалить элемент?";
            const string caption = "Удаление элемента из списка";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ListViewItem item = listView1.SelectedItems[0];

                ListMaterials.RemoveAt(item.Index);

                item.Remove();

                FileIO file = new FileIO();
                file.SaveData(ListMaterials);

                LoadList();
            }
        }

        /// <summary>
        /// Изменение данных в списке.
        /// </summary>
        private void RenameButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)       // Если Item не выбран - возвращаяемся
                return;

            AddMaterialForm materialForm = new AddMaterialForm();
            materialForm.Text = "Изменить";
            materialForm.button1.Text = "Изменить";

            ListViewItem item = listView1.SelectedItems[0];

            materialForm.textBoxMaterial.Text = ListMaterials[item.Index].Materials;
            materialForm.textBoxCost.Text = ListMaterials[item.Index].Cost.ToString();

            int index = item.Index;

            if (materialForm.ShowDialog() != DialogResult.OK)
                return;

            if (materialForm.textBoxMaterial.Text != "" && materialForm.textBoxCost.Text != "")
            {
                decimal cost;

                if (decimal.TryParse(materialForm.textBoxCost.Text, out cost))
                {
                    ListMaterials[index].Materials = materialForm.textBoxMaterial.Text;
                    ListMaterials[index].Cost = cost;                                      

                    FileIO file = new FileIO();
                    file.SaveData(ListMaterials);
                                        
                    LoadList();
                }
                else
                    MessageBox.Show("Некорректный ввод данных");
            }
        }
    }
}
