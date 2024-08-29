namespace AutoLayerGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEntityName.Text)) return; 
            listBox1.Items.Add(txtEntityName.Text);
            txtEntityName.Text = "";
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string entityPath = AppDomain.CurrentDomain.BaseDirectory + $"Files\\Entites";
            if (!Directory.Exists(entityPath))
                Directory.CreateDirectory(entityPath);

            string dataAccessPath = AppDomain.CurrentDomain.BaseDirectory + $"\\Files\\DataAccess";
            if (!Directory.Exists(dataAccessPath + "\\Abstract"))
                Directory.CreateDirectory(dataAccessPath + "\\Abstract");
            if (!Directory.Exists(dataAccessPath + "\\Concrete"))
                Directory.CreateDirectory(dataAccessPath + "\\Concrete");
            if (!Directory.Exists(dataAccessPath + "\\Mapping"))
                Directory.CreateDirectory(dataAccessPath + "\\Mapping");


            string businessPath = AppDomain.CurrentDomain.BaseDirectory + $"\\Files\\Business";
            if (!Directory.Exists(businessPath + "\\Abstract"))
                Directory.CreateDirectory(businessPath + "\\Abstract");
            if (!Directory.Exists(businessPath + "\\Concrete"))
                Directory.CreateDirectory(businessPath + "\\Concrete");

            string controllerPath = AppDomain.CurrentDomain.BaseDirectory + $"\\Files\\Controller";
            if (!Directory.Exists(controllerPath))
                Directory.CreateDirectory(controllerPath);

            foreach (var item in listBox1.Items)
            {
                #region Entity
                string data = "namespace CoraxwidePro.Entities.Concrete\r";
                data += "{\r";
                data += $"public class {item} : IBaseEntity\r";
                data += "{\r";
                data += "}\r";
                data += "}";
                File.WriteAllText(entityPath + $"\\{item}.cs", data);
                #endregion

                #region DataAccess
                data = "namespace CoraxwidePro.DataAccess.Abstract\r";
                data += "{\r";
                data += $"public interface I{item}Dal : IRepository<{item}>\r";
                data += "{\r";
                data += "}\r";
                data += "}";
                File.WriteAllText(dataAccessPath + "\\Abstract" + $"\\I{item}Dal.cs", data);

                data = "namespace CoraxwidePro.DataAccess.Concrete\r";
                data += "{\r";
                data += $"public class Ef{item}Dal : EfRepository<{item}, CoraxwideProContext>, I{item}Dal\r";
                data += "{\r";
                data += "}\r";
                data += "}";
                File.WriteAllText(dataAccessPath + "\\Concrete" + $"\\Ef{item}Dal.cs", data);

                data = "namespace CoraxwidePro.DataAccess.Concrete.EntityFramework.Mappings\r";
                data += "{\r";
                data += $"public class {item}Map : BaseEntityMap<{item}>\r";
                data += "{\r";
                data += $"        public override void Configure(EntityTypeBuilder<{item}> builder)\r";
                data += "{\r";
                data += $"            builder.ToTable(\"{item}s\", \"dbo\");\r";
                data += $"            builder.Property(x => x.Name).HasColumnName(\"Name\");\r";
                data += "}\r";
                data += "}\r";
                data += "}";
                File.WriteAllText(dataAccessPath + "\\Mapping" + $"\\{item}Map.cs", data);
                #endregion

                #region Business
                data = "namespace CoraxwidePro.Business.Abstract\r";
                data += "{\r";
                data += $"public interface I{item}Service\r";
                data += "{\r";
                data += "}\r";
                data += "}";
                File.WriteAllText(businessPath + "\\Abstract" + $"\\I{item}Service.cs", data);

                data = "namespace CoraxwidePro.Business.Concrete\r";
                data += "{\r";
                data += $"public class {item}Manager : I{item}Service\r";
                data += "{\r";
                data += "}\r";
                data += "}";
                File.WriteAllText(businessPath + "\\Concrete" + $"\\{item}Manager.cs", data);
                #endregion

                #region Controller
                data = "using Microsoft.AspNetCore.Mvc;\r";
                data = "\r";
                data = "namespace CoraxwidePro.Api.Controllers\r";
                data += "{\r";
                data += "    [Route(\"api/[controller]\")]\r";
                data += "    [ApiController]\r";
                data += $"   public class {item}sController : ControllerBase\r";
                data += "{\r";
                data += "}\r";
                data += "}";
                File.WriteAllText(controllerPath + $"\\{item}sController.cs", data); 
                #endregion
            }
            MessageBox.Show("Ýþlem tamam");
        }
    }
}