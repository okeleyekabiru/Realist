import { Component, Input, OnInit} from '@angular/core';
import { IBalance } from '../IUserDetails';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { AccountBalance } from '../UserDetails';
import { Router } from '@angular/router';


@Component({
    selector: 'pm-balance',
    templateUrl:"./BalanceComponent.html"
})
export class BalanceComponent implements OnInit {
    constructor(private http:HttpClient,private router:Router ){}
    ngOnInit(): void {
        this.GetBalance();
    }

    account: IBalance
    errorMessage:string

    GetBalance = () =>
    {
        const body = {
            AccountNumber:history.state.account
        }
        let authToken = JSON.parse(localStorage.getItem("access_token")).token;
        let headers = {
          headers: new HttpHeaders({
            Authorization: `Bearer ${authToken}`
          })
        };
    
        return this.http.post("http://localhost:5000/api/validate/balance", body, headers).pipe(map((result) => {
            this.account = new AccountBalance(result["balance"], result["accountNumber"])
            console.log(this.account)
        })).subscribe(result => {
            return result
        }, e => {
                this.errorMessage = "cannot get balance"
        this.router.navigate(["/cardverification"])
    } )
    } 
}