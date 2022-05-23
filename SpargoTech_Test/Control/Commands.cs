using System;
using System.Data;
using SpargoTech_Test.Configuration;
using SpargoTech_Test.Data;
using System.Collections.Generic;

namespace SpargoTech_Test.Control
{
    public class Commands
    {
        SQLWorker sql = new SQLWorker();

        /// <summary>
        /// Создать товар
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddProduct (string name)
        {
            if (name.Length != 0)
            {
                var count = sql.ExecuteNonQuery("INSERT INTO Products (ProductName)"
                                                + $"SELECT ('{name}')"
                                                + $"WHERE NOT EXISTS (SELECT ProductName FROM Products WHERE ProductName = '{name}')");
                if (count != 0)
                    return "Товар добавлен";
                else
                    return "Невозможно добавить товар";
            }
            else
                return "Невозможно добвыить товар без названия";    
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string DeleteProduct(string name)
        {
            if (name.Length != 0)
            {
                var count = sql.ExecuteNonQuery($"DELETE FROM Batches WHERE ProductID = (SELECT ProductID FROM Products WHERE ProductName = '{name}'); "
                                            + $"DELETE FROM Products WHERE ProductName = '{name}'");
                if (count != 0)
                    return "Товар удален";
                else
                    return "Невозможно удалить товар";
            }
            else
                return "Невозможно удалить товар без названия";
        }

        /// <summary>
        /// Добавить аптеку
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string AddPharmacy(string name, string address, string phone)
        {
            if (name.Length != 0 && address.Length != 0 && phone.Length!=0)
            {
                var count = sql.ExecuteNonQuery("INSERT INTO Pharmacies (PharmacyName, PharmacyAddress, PharmacyPhone)"
                                        + $"SELECT '{name}', '{address}', '{phone}'"
                                        + $"WHERE NOT EXISTS (SELECT PharmacyName FROM Pharmacies WHERE PharmacyName = '{name}')");
                if (count != 0)
                    return "Аптека добавлена";
                else
                    return "Невозможно добавить аптеку";
            }
            else
                return "Недостаточно данных";
        }

        /// <summary>
        /// Удалить аптеку
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string DeletePharmacy(string name)
        {
            if (name.Length != 0)
            {
                var dt = sql.SelectRequest($"SELECT WarehouseID FROM Warehouses WHERE PharmacyID = (SELECT PharmacyID FROM Pharmacies WHERE PharmacyName = '{name}')");
                var ids = new List<string> { };
                foreach (DataRow r in dt.Rows)
                    ids.Add(r[0].ToString());
                var count = sql.ExecuteNonQuery($"DELETE FROM Batches WHERE WarehouseID IN ({string.Join(", ", ids)});"
                                              + $"DELETE FROM Warehouses WHERE PharmacyID = (SELECT PharmacyID FROM Pharmacies WHERE PharmacyName = '{name}');"
                                              + $"DELETE FROM Pharmacies WHERE PharmacyName = '{name}'");
                if (count != 0)
                    return "Аптека удалена";
                else
                    return "Невозможно удалить аптеку";
            }
            else
                return "Невозможно удалить аптеку без названия";
        }

        /// <summary>
        /// Добавить склад
        /// </summary>
        /// <param name="pharmacyName"></param>
        /// <param name="warehouseName"></param>
        /// <returns></returns>
        public string AddWarehouse(string pharmacyName, string warehouseName)
        {
            if (warehouseName.Length != 0)
            {
                var count = sql.ExecuteNonQuery($"INSERT INTO Warehouses (PharmacyId, WarehouseName)"
                                          + $"SELECT (SELECT PharmacyId FROM Pharmacies WHERE PharmacyName = '{pharmacyName}'), '{warehouseName}'"
                                          + $"WHERE NOT EXISTS (SELECT WarehouseName FROM Warehouses WHERE WarehouseName = '{warehouseName}')");
                if (count != 0)
                    return "Склад добавлен";
                else
                    return "Невозможно добавить склад";
            }
            else
                return "Невозможно добавить склад без названия";
        }    

        /// <summary>
        /// Удалить склад
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string DeleteWarehouse(string name)
        {
            if (name.Length != 0)
            {
                var count = sql.ExecuteNonQuery($"DELETE FROM Batches WHERE WarehouseID = (SELECT WarehouseID FROM Warehouses WHERE WarehouseName = '{name}'); "
                                           + $"DELETE FROM Warehouses WHERE WarehouseName = '{name}'");
                if (count != 0)
                    return "Склад удален";
                else
                    return "Невозможно удалить склад";
            }
            else
                return "Невозможно удалить склад без названия";
        }

        /// <summary>
        /// Добавить партию
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="warehouseName"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public string AddBatch(string productName, string warehouseName, int quantity)
        {
            var count = sql.ExecuteNonQuery("INSERT INTO Batches (ProductID, WarehouseID, Quantity) VALUES "                                          
                                         + $"((SELECT ProductID FROM Products WHERE ProductName = '{productName}'), "
                                         + $"(SELECT WarehouseId FROM Warehouses WHERE WarehouseName = '{warehouseName}'), "
                                         + $"{quantity})");          
            if (count != 0)
                return "Партия добавлена";
            else
                return "Невозможно добавить партию";
        }

        /// <summary>
        /// Удалить партию
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public string DeleteBatch(string id)
        {
            var count = sql.ExecuteNonQuery($"DELETE FROM Batches WHERE BatchID = '{id}'");                                          
            if (count != 0)
                return "Партия удалена";
            else
                return "Невозможно удалить партию";
        }

        /// <summary>
        /// Получить список всех товаров и его количество в выбранной аптеке
        /// </summary>
        /// <param name="pharmacyName"></param>
        /// <returns></returns>
        public DataTable SelAllProduct(string pharmacyName)
        {
            var dt = sql.SelectRequest($"SELECT WarehouseID FROM Warehouses WHERE PharmacyID = (SELECT PharmacyID FROM Pharmacies WHERE PharmacyName = '{pharmacyName}')");
            var warehouseIds = new List<string> { };
            foreach (DataRow r in dt.Rows)
                warehouseIds.Add(r[0].ToString());
            return sql.SelectRequest("SELECT p.ProductName, s.Summa " 
                                  + $"FROM (SELECT ProductID, SUM(Quantity) as Summa FROM Batches WHERE WarehouseID IN ({string.Join(", ", warehouseIds)}) GROUP BY ProductID) s "
                                   + "LEFT JOIN Products p ON p.ProductID = s.ProductID");
             
                 
        }
    }
}