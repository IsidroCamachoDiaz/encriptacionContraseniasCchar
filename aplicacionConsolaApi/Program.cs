using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using JsonSerializer = System.Text.Json.JsonSerializer;

var url = "http://localhost:5099/api/Usuarios";
JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive=true};
using (var httpdClient = new HttpClient())
{
    var response = await httpdClient.GetAsync(url);
    if (response.IsSuccessStatusCode)
    {
        var conten =await response.Content.ReadAsStringAsync();
        var usuario = JsonSerializer.Deserialize<List<Usuario>>(conten,options);
        Console.WriteLine("Introduzca usuario");
        var nombre = Console.ReadLine();

        foreach(var item in usuario)
        {
            if (nombre == item.nombre_usuario)
            {
                Console.WriteLine("Introduzca contrasenia");
                var contrasenia=Console.ReadLine();
                contrasenia = encript.EncryptString(contrasenia, "prueba");
                if(contrasenia== encript.EncryptString(item.clave_usuario, "prueba"))
                {
                    Console.WriteLine("La contraseña esta bien");
                }
                else
                    Console.WriteLine("La contrseña esta mal");
            }
            /*using (HttpClient httpClient = new HttpClient())
            {
                var url2 = "http://localhost:5099/api/Usuarios/" + item.id_usuario;

                // Crear una instancia del objeto que deseas actualizar

                // Serializa el objeto a JSON
                var requestBody = JsonConvert.SerializeObject(item);

                var request = new HttpRequestMessage(HttpMethod.Put, url2);

                // Agrega el contenido serializado al cuerpo de la solicitud
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("La solicitud PUT se completó con éxito.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }*/

        }
    }
    else
        Console.WriteLine("Error");
}
    Console.ReadKey();

public class Acceso
{
    public long id_acceso { get; set; }
    public string codigo_acceso { get; set; }
    public string descripcion_acceso { get; set; }
}

public class Usuario
{
    public long id_usuario { get; set; }
    public string dni_usuario { get; set; }
    public string nombre_usuario { get; set; }
    public string apellidos_usuario { get; set; }
    public string tlf_usuario { get; set; }
    public string email_usuario { get; set; }
    public string clave_usuario { get; set; }
    public long id_acceso { get; set; }
    public Acceso acceso { get; set; }
    public bool estaBloqueado_usuario { get; set; }
    public DateTime fch_fin_bloqueo_usuario { get; set; }
    public DateTime fch_alta_usuario { get; set; }
    public DateTime fch_baja_usuario { get; set; }
}
public class encript
{
    public static string EncryptString(string text, string key)
    { 
        using (Aes aesAlg = Aes.Create())
        {
            if (key.Length < 16)
            {
                key = key.PadRight(16, ' '); // Agregar espacios en blanco para alcanzar la longitud requerida
            }
            else if (key.Length > 16)
            {
                key = key.Substring(0, 16); // Truncar la clave si es más larga
            }
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.Mode = CipherMode.ECB; // Establece el modo ECB

            ICryptoTransform encryptor = aesAlg.CreateEncryptor();

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                }

                // Convierte los bytes cifrados en una cadena Base64
                byte[] encryptedBytes = msEncrypt.ToArray();
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }
}


public class IVGenerator
{
    public static byte[] GenerateRandomIV(int sizeInBytes)
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] iv = new byte[sizeInBytes];
            rng.GetBytes(iv);
            return iv;
        }
    }
}