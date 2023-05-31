<?php

$privateKeyPEM = file_get_contents('../private_key.pem');
$privateKey = openssl_pkey_get_private($privateKeyPEM);
$partnerId = $argv[1];
$rpps = $argv[2];
$dateTime = date('Y-m-d H:i:s');

$dataToSign = [
    'partnerId' => $partnerId,
    'rpps' => $rpps,
    'dateTime' => $dateTime
];
$dataToSignJson = json_encode($dataToSign);
// Calculer la signature
$signature = '';
openssl_sign($dataToSignJson, $signature, $privateKey, OPENSSL_ALGO_SHA512);
// Convertir la signature en format base64
$signatureBase64 = base64_encode($signature);

$dataSigned = array_merge($dataToSign, ['sig' => $signatureBase64]);
$dataSignedJson = json_encode($dataSigned);

$url = 'https://partners.test.ordoclic.fr/v1/auth';
$options = [
    'http' => [
        'method' => 'POST',
        'header' => 'Content-Type: application/json',
        'content' => $dataSignedJson
    ]
];
$context = stream_context_create($options);
$response = file_get_contents($url, false, $context);
echo json_encode($response);

?>
