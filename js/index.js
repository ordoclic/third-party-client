const fs = require('fs');
const fetch = require('node-fetch');
const NodeRSA = require('node-rsa');
const privateKeyPEM = fs.readFileSync("../private_key.pem");

// 1. Decode the private key with base64 then pkcs8
const key = new NodeRSA(privateKeyPEM, 'pkcs1');
//key.importKey(privateKeyPEM, 'pkcs8'); 
const privateKey = key.exportKey();

// 2. Sign the data with the decoded private key and sha256 then encode it with base64
const partnerId = process.argv[2];
const rpps = process.argv[3];
const dateTime = new Date();

const dataToSign = {
    partnerId,
    rpps,
    dateTime
};


const signature = new NodeRSA(privateKey, {signingScheme: 'sha512'}).sign(JSON.stringify(dataToSign)).toString('base64');
const dataSigned = { ...dataToSign, sig: signature };
console.log(JSON.stringify(dataSigned));

const result = fetch('https://partners.staging.ordoclic.fr/v1/auth', {
    method: 'POST',
    headers: {
    'Content-Type': 'application/json',
    },
    body: JSON.stringify(dataSigned)
})
.then(res => res.json())
.then((data) => data);

result.then(response => console.log(response));

 
