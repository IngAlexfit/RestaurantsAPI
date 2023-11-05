namespace Restaurants_From_Colombia.Services
{
    using Restaurants_From_Colombia.Model;
    using MongoDB.Driver;
    using Restaurants_From_Colombia.BD;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(MongoDBSettings mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.MongoDBConnection);
            var database = client.GetDatabase("restaurantManagement");
            _usersCollection = database.GetCollection<User>("usuarios");
        }

        public User Authenticate(string username, string password)
        {
            var user = _usersCollection.Find(u => u.Username == username).SingleOrDefault();

            if (user == null || !VerifyPasswordHash(password, user.Password))
                return null;

            return user;
        }

        public User GetById(string id)
        {
            return _usersCollection.Find(u => u._Id.ToString() == id).SingleOrDefault();
        }

        public User Create(User user, string password)
        {
            // Validaciones y almacenamiento seguro de contraseña
            // Asegúrate de implementar esto de manera segura
            user.Password = CreateMD5PasswordHash(password); // Cambio al método que utiliza MD5
            _usersCollection.InsertOne(user);

            return user;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Verifica que el hash MD5 de la contraseña coincida con el almacenado
            // Asegúrate de implementar esto de manera segura
            string passwordHash = CreateMD5PasswordHash(password); // Crea el hash MD5 de la contraseña a verificar
            return passwordHash == storedHash;
        }

        private string CreateMD5PasswordHash(string password)
        {
            // Genera un hash MD5 seguro a partir de la contraseña
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convierte los bytes del hash en una representación hexadecimal
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

    }

}
