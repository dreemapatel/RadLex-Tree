using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;

namespace RadLexTreeLazy
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	
	public partial class MainWindow : Window
	{
		public OleDbConnection database;
		public MainWindow()
		{
			InitializeComponent();

		}
		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			String connectionString;
			connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\TeraRecon\\Documents\\Visual Studio 2012\\Projects\\RadlexTreeLazy\\RadlexTreeLazy\\bin\\Debug;Extended Properties=\"text;HDR=yes;FMT=Delimited(,)\"";
			database = new OleDbConnection(connectionString);

			database.Open();

			
			TreeViewItem root = new TreeViewItem();
			root.Tag = "anatomical entity";
			root.Header = "anatomical entity";
			root.Items.Add("Loading...");
			
				treestruct.Items.Add(root);
			

		}
		public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = e.Source as TreeViewItem;
			if (item != null)
			{
				if ((item.Items.Count == 1) && (item.Items[0] is string))
				{
					item.Items.Clear();
					string pname = item.Tag.ToString();
					String childquery = "SELECT RID from radlexcs.csv where Term ='" + pname + "'";

					OleDbCommand SQLQuery = new OleDbCommand(childquery, database);
					
					SQLQuery.CommandText = childquery;
					SQLQuery.Connection = database;
					string parentId=SQLQuery.ExecuteScalar().ToString();

					String query = "SELECT Term,RID from radlexcs.csv where Parent_RID ='" + parentId + "'";
					OleDbCommand Query = new OleDbCommand(query, database);
					DataTable data = new DataTable();
					

					OleDbDataAdapter dataAdapter = new OleDbDataAdapter(Query);
					dataAdapter.Fill(data);
					try
					{
						if (data != null)
						{
							foreach (DataRow rdr in data.Rows)
							{
								TreeViewItem child = new TreeViewItem();
								child.Tag = rdr["Term"].ToString();
								child.Header = rdr["Term"].ToString();
								child.Items.Add("Loading..");
								item.Items.Add(child);
							}
						}
					}
					catch (Exception ex)
					{
					}
			
				}
			}
		}


	}
}
