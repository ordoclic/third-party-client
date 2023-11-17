Java 18 and Maven is required

## Quick java setup with [SDKMAN](https://sdkman.io/)

```bash
curl -s "https://get.sdkman.io" | bash
source "$HOME/.sdkman/bin/sdkman-init.sh"
sdk install java 18-open
sdk install maven
```

## Run Java project

- args[0] -> PEM path
- args[1] -> partnerId
- args[2] -> rpps

```bash
mvn install
mvn exec:java -Dexec.mainClass="com.ordoclic.auth.App" -Dexec.args="/path/to/yourcert.pem 40d8aa4c-617e-47dc-8abe-aa27163e6b04 56660006661"
```
