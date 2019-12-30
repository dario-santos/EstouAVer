using System.Security.Cryptography;
using System.Runtime.InteropServices;

[DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
public static extern bool ZeroMemory(IntPtr Destination, int Length);

//gera um salt aleatório que será usado para cifrar o ficherio da base de dados
public static byte[] generateRandomSalt(){

    byte[] data = new byte[32];

    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()){

        for(int i = 0; i < 10; i++)
            rng.GetBytes(data);
    }

    return data;
}

//cifra ficheiro com a password dada
//feito com ajuda de 
//http://stackoverflow.com/questions/27645527/aes-encryption-on-large-files
private void FileEncrypt(string inputFile, string password){

    byte[] salt = generateRandomSalt();

    FileStream fs = new FileStream(inputFile + ".aes", FileMode.Create);

    //converter string pass para byte array
    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

    RijndaelManaged AES = new RijndaelManaged();
    AES.KeySize = 256;
    AES.BlockSize = 128;
    AES.Padding = PaddingMode.PKCS7;

    var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
    AES.Key = key.GetBytes(AES.KeySize / 8);
    AES.IV = key.GetBytes(AES.BlockSize / 8);

    //usamos o modo CBC porque é pedido no enunciado do projeto
    //segundo certas fontes o CFB é mais seguro
    AES.Mode = CipherMode.CBC;

    fs.Write(salt, 0, salt.Length);

    CryptoStream cd = new CryptoStream(fs, AES.CreateEncryptor(), CryptoStreamMode.Write);

    FileStream fsIn = new FileStream(inputFile, FileMode.Open);

    //criar buffer de 1Mb para alocar só este tamanho na mamória, e ão o ficheiro todo
    byte[] buffer = new byte[1048576];

    int readFile;

    try{
        while ((readFile = fsIn.Read(buffer, 0, buffer.Length)) > 0) {

            Application.DoEvents();
            cs.Write(buffer, 0, readFile);
        }

        fsIn.Close();
    }

    catch (Exception ex){
        Console.WriteLine("Error: " + ex.Message);
    }

    finally{
        cs.Close();
        fs.Close();
    }
}

//decifra um ficheiro cifrado através do método FileEncrypt
private void FileDecrypt(string inputFile, string outputFile, string password){

    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
    byte[] salt = new byte[32];

    FileStream fs = new FileStream(inputFile, FileMode.Open);
    fs.Read(salt, 0, salt.Length);

    RijndaelManaged AES = new RijndaelManaged();
    AES.KeySize = 256;
    AES.BlockSize = 128;
    var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
    AES.Key = key.GetBytes(AES.KeySize / 8);
    AES.IV = key.GetBytes(AES.BlockSize / 8);
    AES.Padding = PaddingMode.PKCS7;
    AES.Mode = CipherMode.CBC;

    CryptoStream cs = new CryptoStream(fs, AES.CreateDecryptor(), CryptoStreamMode.Read);

    FileStream fsout = new FileStream(outputFile, FileMode.Create);

    int readFile;
    byte[] buffer = new byte[1048576];

    try{
        while ((readFile = cs.Read(buffer, 0, buffer.Length)) > 0 ){

            Application.DoEvents();
            fsout.Write(buffer, 0, readFile);
        }
    }

    catch (CryptographicException ex_Crypto) {
        Console.WriteLine("CryptographicException error: " + ex_Crypto);
    }

    catch (Exception ex) {
        Console.WriteLine("Error: " + ex.Message);
    }

    try{
        cs.Close();
    }

    catch (Exception ex) {
        Console.WriteLine("Error when closing CryptoStream: " + ex.Message);
    }

    finally{
        fsout.Close();
        fs.Close();
    }
}