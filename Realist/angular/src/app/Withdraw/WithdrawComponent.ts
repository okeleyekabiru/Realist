import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { Router } from '@angular/router';
import { AccountDetails } from "../AccountDetails";

@Component({ 
    templateUrl: './WithdrawComponent.html',
    selector: "pm-withdraw"
})
export class WithdrawConponent{
    constructor(private http:HttpClient,private router:Router){}
    Amount: number
    errorMessage: string
    Account:any
    
    
    OnSubmit = (Amount) => {
        
        const body = {
            AccountNumber: history.state.account,
            Amount:Number(Amount)
        }
    
        
        let authToken = JSON.parse(localStorage.getItem("access_token")).token;
        let headers = {
          headers: new HttpHeaders({
            Authorization: `Bearer ${authToken}`
          })
        };

        return this.http
          .post("http://localhost:5000/api/validate/withdraw", body, headers)
          .pipe(
            map(result => {
                this.Account = new AccountDetails(
                    result["id"],
                    result["accountNumber"],
                    result["accountType"],
                    result["balance"],
                    result["dateCreated"],
                    result["isActive"]
                );
                if (this.Account.fullName == undefined || this.Account.fullName == null && this.Account.accountNumber == undefined || this.Account.accountNumber == null) {
              
                    return false
                }
              
                return true
            })
          )
          .subscribe(
              result => {
            },
              e => {
                  this.errorMessage = "error occured while fetching result";
           this.router.navigate(["/cardverification"])
                  
            }
        );
        


    }

}