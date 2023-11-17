package com.ordoclic.auth;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonInclude.Include;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;

@JsonPropertyOrder({ "partnerId", "rpps", "dateTime", "sig" })
public class AuthRequestDTO {
    @JsonProperty("partnerId")
    private String partnerId;
    @JsonProperty("rpps")
    private String rpps;
    @JsonProperty("dateTime")
    private String dateTime;
    @JsonProperty("sig")
    @JsonInclude(Include.NON_NULL)
    private String signature;

    AuthRequestDTO(String partnerId, String rpps, String dateTime) {
        this.partnerId = partnerId;
        this.rpps = rpps;
        this.dateTime = dateTime;
    }

    // Getter Methods
    public String getPartnerId() {
        return partnerId;
    }
    public String getRpps() {
        return rpps;
    }
    public String getDateTime() {
        return dateTime;
    }
    public String getSignature() {
        return signature;
    }

    // Setter Methods
    public void setPartnerId(String partnerId) {
        this.partnerId = partnerId;
    }
    public void setRpps(String rpps) {
        this.rpps = rpps;
    }
    public void setDateTime(String dateTime) {
        this.dateTime = dateTime;
    }
    public void setSignature(String signature) {
        this.signature = signature;
    }
}
