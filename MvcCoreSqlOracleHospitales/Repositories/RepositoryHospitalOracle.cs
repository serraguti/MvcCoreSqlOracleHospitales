using MvcCoreSqlOracleHospitales.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#region PROCEDIMIENTOS ALMACENADOS
//select* from hospital;

//create or replace procedure SP_DELETE_HOSPITAL
//(p_idhospital hospital.hospital_cod%type)
//as
//begin
//  delete from hospital where hospital_cod=p_idhospital;
//commit;
//end;

//create or replace procedure SP_UPDATE_HOSPITAL
//(p_idhospital hospital.hospital_cod%type
//, p_nombre hospital.nombre%type
//, p_direccion hospital.direccion%type
//, p_telefono hospital.telefono%type
//, p_camas hospital.num_cama%type)
//as
//begin
//  update hospital set nombre=p_nombre, direccion = p_direccion
//  , telefono = p_telefono, num_cama = p_camas
//  where hospital_cod=p_idhospital;
//commit;
//end;

//create or replace procedure SP_INSERT_HOSPITAL
//(p_idhospital hospital.hospital_cod%type
//, p_nombre hospital.nombre%type
//, p_direccion hospital.direccion%type
//, p_telefono hospital.telefono%type
//, p_camas hospital.num_cama%type)
//as
//begin
//  insert into hospital values 
//  (p_idhospital, p_nombre, p_direccion, p_telefono, p_camas);
//commit;
//end;

#endregion

namespace MvcCoreSqlOracleHospitales.Repositories
{
    public class RepositoryHospitalOracle : IRepositoryHospital
    {
        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable tablaHospital;

        public RepositoryHospitalOracle()
        {
            string connectionString =
                 @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from hospital";
            this.adapter = new OracleDataAdapter(sql, this.cn);
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

        public void DeleteHospital(int idhospital)
        {
            OracleParameter pamid = new OracleParameter("p_idhospital", idhospital);
            this.com.Parameters.Add(pamid);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_HOSPITAL";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        private int GetMaxIdHospital()
        {
            int maximo = (from datos in this.tablaHospital.AsEnumerable()
                          select datos).Max(z => z.Field<int>("HOSPITAL_COD")) + 1;
            return maximo;
        }

        public void InsertHospital(string nombre, string direccion, string telefono, int camas)
        {
            int maximo = this.GetMaxIdHospital();
            OracleParameter pamid = new OracleParameter("p_idhospital", maximo);
            this.com.Parameters.Add(pamid);
            OracleParameter pamnombre = new OracleParameter("p_nombre", nombre);
            this.com.Parameters.Add(pamnombre);
            OracleParameter pamdir = new OracleParameter("p_direccion", direccion);
            this.com.Parameters.Add(pamdir);
            OracleParameter pamtlf = new OracleParameter("p_telefono", telefono);
            this.com.Parameters.Add(pamtlf);
            OracleParameter pamcamas = new OracleParameter("p_camas", camas);
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
            OracleParameter pamid = new OracleParameter("p_idhospital", idhospital);
            this.com.Parameters.Add(pamid);
            OracleParameter pamnombre = new OracleParameter("p_nombre", nombre);
            this.com.Parameters.Add(pamnombre);
            OracleParameter pamdir = new OracleParameter("p_direccion", direccion);
            this.com.Parameters.Add(pamdir);
            OracleParameter pamtlf = new OracleParameter("p_telefono", telefono);
            this.com.Parameters.Add(pamtlf);
            OracleParameter pamcamas = new OracleParameter("p_camas", camas);
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
