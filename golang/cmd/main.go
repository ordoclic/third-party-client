package main

import (
	"bytes"
	"encoding/json"
	"log"
	"net/http"
	"os"
	"third-party-client/internal/crypto"
	"time"

	"github.com/sirupsen/logrus"
	"github.com/spf13/afero"
)

var (
	PartnerID string
	Rpps      string
	DateTime  string
)

// AuthRequest contain data to get professional third party auth response for trusted partner
type AuthRequest struct {
	*ProfessionalData
	Sig []byte `json:"sig"`
}

// ProfessionalData contain necessary data to make trusted partner auth signature
type ProfessionalData struct {
	PartnerID string `json:"partnerId"`
	Rpps      string `json:"rpps"`
	DateTime  string `json:"dateTime"`
}

func init() {
	PartnerID = os.Args[1]
	Rpps = os.Args[2]
}

func main() {
	fs := afero.NewOsFs()
	// build signature models
	modelShouldBeSign := ProfessionalData{
		PartnerID: PartnerID,
		Rpps:      Rpps,
		DateTime:  time.Now().Format(time.RFC3339),
	}
	contentToSign, err := json.Marshal(modelShouldBeSign)
	if err != nil {
		panic(err)
	}
	bytePrivateKey, err := afero.ReadFile(fs, "../private_key.pem")
	if err != nil {
		panic(err)
	}
	privateKey, err := crypto.BytesToPrivateKey(bytePrivateKey)
	if err != nil {
		panic(err)
	}
	hash, err := crypto.SignWithPrivateKey(contentToSign, privateKey)
	if err != nil {
		panic(err)
	}
	body := AuthRequest{
		ProfessionalData: &modelShouldBeSign,
		Sig:              hash,
	}
	bytesRepresentation, err := json.Marshal(body)
	if err != nil {
		log.Fatalln(err)
	}
	resp, err := http.Post("https://partners.staging.ordoclic.fr/v1/auth", "application/json", bytes.NewBuffer(bytesRepresentation))
	if err != nil {
		log.Fatalln(err)
	}
	var result map[string]interface{}
	json.NewDecoder(resp.Body).Decode(&result)
	logrus.Infof("result : %s", result)
}
