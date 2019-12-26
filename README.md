# EstouAVer
Trabalho de grupo desenvolvido para a Unidade Curricular Segurança de Informática

## Serviço (Daemon)

Os comandos de inicialização do serviço devem ser realizados pela linha de comandos (como administrador), o powershell não suporta o comando sc. 

### Instalar o serviço:
```
sc create EstouAVerService binpath= C:\path\completo\para\o\executal\EstouAVer.exe 
```

### Desinstalar o serviço

```Powershell
sc delete EstouAVerService 
```

### Começar o serviço

```Powershell
sc start EstouAVerService username password directory
```

### Parar o serviço

```Powershell
sc stop EstouAVerService
```