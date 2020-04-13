import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { NavBarComponent } from "./NavBar/navbar.component";
import { FormsModule } from "@angular/forms";
import { CardVerification } from "./CardVerification/cardverification.component";
import { HttpClientModule } from "@angular/common/http";
import { PinComponent } from "./CardVerification/Pin.confirmation.component";
import { RouterModule, Routes } from "@angular/router";
import { TransactonComponent } from "./Transaction/TransactionComponent";
import { AppRoute } from "./CardVerification/AppRoute.Component";
import { BalanceComponent } from "./Balance/BalanceComponent";
import { WithdrawConponent } from './Withdraw/WithdrawComponent';
import { DepositComponent } from './Withdraw/DepositComponent';
@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    CardVerification,
    PinComponent,
    TransactonComponent,
    AppRoute,
    BalanceComponent,
    WithdrawConponent,
    DepositComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(
      [
        { path: "cardverification", component: AppRoute },
        { path: "transaction", component: TransactonComponent },
        { path: "withdraw", component: WithdrawConponent },
        { path: "deposit", component: DepositComponent },
        { path: "balance", component: BalanceComponent },
        { path: "", redirectTo: "cardverification", pathMatch: "full" },
        { path: "**", redirectTo: "cardverification", pathMatch: "full" }
      ],
      { useHash: true }
    )
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
