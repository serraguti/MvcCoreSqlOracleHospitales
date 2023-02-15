using MvcCoreSqlOracleHospitales.Models;
using System.Data;
using System.Data.SqlClient;

#region PROCEDIMIENTOS ALMACENADOS
//CREATE PROCEDURE SP_INSERT_HOSPITAL
//(@ID INT, @NOMBRE NVARCHAR(50)
//, @DIRECCION NVARCHAR(50), @TELEFONO NVARCHAR(10)
//, @CAMAS INT)
//AS
//    INSERT INTO HOSPITAL VALUES
//	(@ID, @NOMBRE, @DIRECCION, @TELEFONO, @CAMAS)
//GO

//CREATE PROCEDURE SP_UPDATE_HOSPITAL
//(@ID INT, @NOMBRE NVARCHAR(50)
//, @DIRECCION NVARCHAR(50), @TELEFONO NVARCHAR(10)
//, @CAMAS INT)
//AS
//    update HOSPITAL set NOMBRE = @NOMBRE, DIRECCION = @DIRECCION
//    , TELEFONO = @TELEFONO, NUM_CAMA = @CAMAS
//	where HOSPITAL_COD=@ID
//GO

//CREATE PROCEDURE SP_DELETE_HOSPITAL
//(@ID INT)
//AS
//    DELETE FROM HOSPITAL
//	where HOSPITAL_COD=@ID
//GO
#endregion

namespace MvcCoreSqlOracleHospitales.Repositories
{
    public class RepositoryHospitalSql : IRepositoryHospital
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataAdapter adapter;
        private DataTable tablaHospital;

        public RepositoryHospitalSql()
        {
            string connectionString =
               @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from hospital";
            this.adapter = new SqlDataAdapter(sql, this.cn);
            this.tablaHospital = new DataTable();
            this.adapter.Fill(this.tablaHospital);
        }

        public List<Hospital> GetHospitales()
        {
            var consulta = from datos in this.tablaHospital.AsEnumerable()
                           select new Hospital
                           {
                               IdHospital = datos.Field<int>("HOSPITAL_COD"),
                               Nombre = datos.Field<string>("NOMBRE"),
                               Direccion = datos.Field<string>("DIRECCION"),
                               Telefono = datos.Field<string>("TELEFONO"),
                               Camas = datos.Field<int>("NUM_CAMA")
                           };
            return consulta.ToList();
        }

        public Hospital FindHospital(int idhospital)
        {
            var consulta = from datos in this.tablaHospital.AsEnumerable()
                           where datos.Field<int>("HOSPITAL_COD") == idhospital
                           select new Hospital
                           {
                               IdHospital = datos.Field<int>("HOSPITAL_COD"),
                               Nombre = datos.Field<string>("NOMBRE"),
                               Direccion = datos.Field<string>("DIRECCION"),
                               Telefono = datos.Field<string>("TELEFONO"),
                               Camas = datos.Field<int>("NUM_CAMA")
                           };
            return consulta.FirstOrDefault();
        }

        private int GetMaxIdHospital()
        {
            int maximo = (from datos in this.tablaHospital.AsEnumerable()
                          select datos).Max(z => z.Field<int>("HOSPITAL_COD")) + 1;
            return maximo;
        }

        public void DeleteHospital(int idhospital)
        {
            SqlParameter pamid = new SqlParameter("@ID", idhospital);
            this.com.Parameters.Add(pamid);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_HOSPITAL";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }


        public void InsertHospital(string nombre, string direccion, string telefono, int camas)
        {
            int maximo = this.GetMaxIdHospital();
            SqlParameter pamid = new SqlParameter("@id", maximo);
            this.com.Parameters.Add(pamid);
            SqlParameter pamnombre = new SqlParameter("@nombre", nombre);
            this.com.Parameters.Add(pamnombre);
            SqlParameter pamdir = new SqlParameter("@direccion", direccion);
            this.com.Parameters.Add(pamdir);
            SqlParameter pamtlf = new SqlParameter("@telefono", telefono);
            this.com.Parameters.Add(pamtlf);
            SqlParameter pamcamas = new SqlParameter("@camas", camas);
            this.com.Parameters.Add(pamcamas);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_HOSPITAL";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void UpdateHospital(int idhospital, string nombre, string direccion, string telefono, int camas)
        {
            SqlParameter pamid = new SqlParameter("@id", idhospital);
            this.com.Parameters.Add(pamid);
            SqlParameter pamnombre = new SqlParameter("@nombre", nombre);
            this.com.Parameters.Add(pamnombre);
            SqlParameter pamdir = new SqlParameter("@direccion", direccion);
            this.com.Parameters.Add(pamdir);
            SqlParameter pamtlf = new SqlParameter("@telefono", telefono);
            this.com.Parameters.Add(pamtlf);
            SqlParameter pamcamas = new SqlParameter("@camas", camas);
            this.com.Parameters.Add(pamcamas);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATE_HOSPITAL";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
