package crypto

import (
	"crypto"
	"crypto/rand"
	"crypto/rsa"
	"crypto/sha512"
	"crypto/x509"
	"encoding/pem"
	"log"
)

// BytesToPrivateKey bytes to private key
func BytesToPrivateKey(priv []byte) (*rsa.PrivateKey, error) {
	block, _ := pem.Decode(priv)
	enc := x509.IsEncryptedPEMBlock(block)
	b := block.Bytes

	var err error
	if enc {
		log.Println("is encrypted pem block")
		b, err = x509.DecryptPEMBlock(block, nil)
		if err != nil {
			return nil, err
		}
	}

	key, err := x509.ParsePKCS1PrivateKey(b)
	if err != nil {
		return nil, err
	}

	return key, nil
}

// SignWithPrivateKey signs data with private key
func SignWithPrivateKey(message []byte, privateKey *rsa.PrivateKey) ([]byte, error) {
	// crypto/rand.Reader is a good source of entropy for blinding the RSA
	// operation.
	rng := rand.Reader

	hashed := hashDigest(message)

	signature, err := rsa.SignPKCS1v15(rng, privateKey, crypto.SHA512, hashed[:])
	if err != nil {
		return nil, err
	}

	return signature, nil
}

func hashDigest(input []byte) []byte {
	hash := sha512.Sum512(input)
	return hash[:]
}
