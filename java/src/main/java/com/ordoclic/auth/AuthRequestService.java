package com.ordoclic.auth;

import java.io.File;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.security.KeyFactory;
import java.security.Signature;
import java.security.interfaces.RSAPrivateKey;
import java.security.spec.PKCS8EncodedKeySpec;
import java.text.SimpleDateFormat;
import java.util.Base64;
import java.util.Date;

import com.fasterxml.jackson.databind.ObjectMapper;

public class AuthRequestService {
    private final static String DATE_PATTERN = "yyyy-MM-dd'T'HH:mm:ss.SSSZ";
    private final static String ODC_AUTH_URL = "https://partners.staging.ordoclic.fr/v1/auth";

    private File certFile;
    private String partnerId;
    private RSAPrivateKey privateKey;

    private ObjectMapper mapper = new ObjectMapper();


    AuthRequestService(final String certFilePath, final String partnerId) throws Exception {
        java.security.Security.addProvider(new org.bouncycastle.jce.provider.BouncyCastleProvider());

        this.certFile = new File(certFilePath);
        privateKey = this.readPKCS8PrivateKey(certFile);
        this.partnerId = partnerId;
    }

    public String getTokenForRPPS(final String rpps) {
        final SimpleDateFormat sdf = new SimpleDateFormat(DATE_PATTERN);
        final String timeStamp = sdf.format(new Date());

        final AuthRequestDTO body = new AuthRequestDTO(this.partnerId, rpps, timeStamp);

        String result = "";
        try {
            final Signature privateSignature = Signature.getInstance("SHA512withRSA");
            privateSignature.initSign(this.privateKey);

            System.out.println("unsigned json:");
            System.out.println(this.mapper.writeValueAsString(body).toString());

            privateSignature.update(this.mapper.writeValueAsString(body).toString().getBytes());

            byte[] signature = privateSignature.sign();
            String signB64 = Base64.getEncoder().encodeToString(signature);
            body.setSignature(signB64);
            String signedPayload = mapper.writeValueAsString(body).toString();

            System.out.println("signed json:");
            System.out.println(signedPayload);
            
            HttpClient httpClient = HttpClient.newHttpClient();
            HttpRequest httpRequest = HttpRequest.newBuilder()
                .uri(new URI(ODC_AUTH_URL))
                .POST(HttpRequest.BodyPublishers.ofString(signedPayload))
                .header("Content-Type", "application/json")
                .build();
            HttpResponse<String> response = httpClient.send(httpRequest, HttpResponse.BodyHandlers.ofString());
            result = response.body();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return result;
    }

    protected RSAPrivateKey readPKCS8PrivateKey(File file) throws Exception {
        String key = new String(Files.readAllBytes(file.toPath()), Charset.defaultCharset());

        String privateKeyPEM = key
                .replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .replaceAll(System.lineSeparator(), "")
                .replace("-----END RSA PRIVATE KEY-----", "");

        byte[] encoded = Base64.getDecoder().decode(privateKeyPEM);

        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        PKCS8EncodedKeySpec keySpec = new PKCS8EncodedKeySpec(encoded);
        return (RSAPrivateKey) keyFactory.generatePrivate(keySpec);
    }
}
