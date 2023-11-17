package com.ordoclic.auth;

public class App {
    public static void main(String[] args) {
        AuthRequestService svc;

        // args[0] -> PEM path
        // args[1] -> partnerId
        // args[2] -> rpps
        try {
            svc = new AuthRequestService(args[0], args[1]);
            String result = svc.getTokenForRPPS(args[2]);

            System.out.println("response:");
            System.out.println(result);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}