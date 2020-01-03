using System;
using System.Security.Cryptography;

namespace RSA{
    
    //classe com os métodos de cifra e decifra
    class EncAndDec{

        public BigInteger c, m;

        public void encrypt(BigInteger m, BigInteger e, BigInteger n){

            c = BigInteger.ModPow(m, e, n);
        }

        public void decrypt(BigInteger cResult, BigInteger d, BigInteger n){
            m = BigInteger.ModPow(cResult, d, n);
        }
    }


    //classe com os métodos utilizados para gerar chaves RSA
    class KeyGen{

        public BigInteger result, d;
        public int f;

        //gera um número aleatório
        public void randomGenerator(){

            RNGCryptoServiceProvider rng = new byte[64];
            byte[] randomNumber = new byte[64];
            rng.GetBytes(randomNumber);
            result = BigInteger.ABbs(result);
        }

        //gera uma chave pública para o RSA
        public BigInteger publicKeyGenerator(BigInteger e, BigInteger n){

            while( e > 1){

                BigInteger ef = BigInteger.Pow(e, f);
                ef = 1 % n;
            }

            return e;
        }

        //gera uma chave privada para o RSA
        public BigInteger privateKeyGenerator(BigInteger e){

            d = (1/e) % f;
        }

        //f(n) = (p-1) * (q-1)
        public void fn(int p, int q){

            return ((p-1)*(q-1));
        }

        //encontra o próximo número primo
        public BigInteger GetNearetPrime(){
            
            while(MillerRabinTest(10) == false){

                result++;
            }
        }

        //testa se o número escolhido é primo ou não
        public bool MillerRabinTest(int n){

            if (result == 2 || result == 3)
                return true;
            
            if (result < 2 || result % 2 == 0)
                return false;

            BigInteger d = result - 1;
            int x = 0;

            while(d % 2 == 0){

                d /= 2;
                x += 1;
            }

            for(var i = 0; i < k; i++){

                RNGCryptoServiceProvider rng = RNGCryptoServiceProvider();
                byte[] _a = new byte[result.ToByteArray().LongLength];
                BigInteger a;

                while(a < 2 || a >= result - 2){

                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }

                BigInteger n = BigInteger.ModPow(a, d, result);

                if(n == 1 || n == result - 1)
                    continue;

                for(var j = 1; j < x; j++){

                    n = BigInteger.ModPow(n, 2, result);

                    if(n == 1)
                        return false;

                    if(n == result - 1)
                        break;
                }
                
                if(n != result - 1)
                    return false;
            }

            return true;
        }
    }
}