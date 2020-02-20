openssl req -newkey rsa:2048 -nodes -keyout ..\Keys\Shopping.key -x509 -days 365 -out ..\Keys\Shopping.cer
openssl pkcs12 -export -in ..\Keys\Shopping.cer -inkey ..\Keys\Shopping.key -out ..\Keys\Shopping.pfx
