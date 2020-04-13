import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { map } from "rxjs/operators";
import { UserDetails } from "../UserDetails";
import { IuserDetails } from '../IUserDetails';
import { Router } from '@angular/router';
@Component({
  selector: "pm-transaction",
  templateUrl: "./TransactionComponent.html"
})
export class TransactonComponent implements OnInit {
    user: IuserDetails;
    
  constructor(private http: HttpClient,private router:Router) {}
  ngOnInit(): void {
    // this.http.get("http://localhost:5000/api/validate/card").subscribe(r => this.fullname = r.name)
     this.GetAccountsDetails(history.state.card )
 
  }

  errormessage: string;
  GetAccountsDetails = card => {
    const body = {
      AtmNumber: card
    };

    let authToken = JSON.parse(localStorage.getItem("access_token")).token;
    let headers = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${authToken}`
      })
    };

    return this.http
      .post("http://localhost:5000/api/validate/dashboard", body, headers)
      .pipe(
        map(result => {
          console.log(result["fullName"]);
          this.user = new UserDetails(
            result["fullName"],
            result["accountNumber"]
            );
            if (this.user.fullName == undefined || this.user.fullName == null && this.user.accountNumber == undefined || this.user.accountNumber == null) {
             console.log(false)
                return false
            }
            console.log(true)
            return true
        })
      )
      .subscribe(
          result => {
        
        },
          e => {
              this.errormessage = "error occured while fetching result";
        this.router.navigate(["/cardverification"])
              
        }
      );
  };
}
