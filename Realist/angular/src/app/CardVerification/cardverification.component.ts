import { Component, ViewChild, Input, OnInit, ElementRef, Renderer2 } from "@angular/core";
import { PinComponent } from './Pin.confirmation.component';

@Component({
    selector: "pm-card",
    templateUrl: "./cardverification.component.html",
    
   
})
export class CardVerification implements OnInit {
  


    constructor(private renderer: Renderer2, private element: ElementRef) {
  
    
    }
    ngOnInit(): void {
        var val = this.element.nativeElement.querySelectorAll("input")
     console.log("running in production.....")
    }
    first4Digits: string
    Last4Digits: string
    Third4Digits: string
    second4Digits: string
    fieldDate: string
    CardHolder: string
    trackcardholder: boolean
    trackcardvalidation: boolean
    cardnumber: string
    expiry: string
    errorMessage:string

   
    flagIfInvalid = (field, isValid) => {
        //	field.classList.toggle("in-invalid",!isValid)
        isValid ? field.classList.remove("is-invalid") : field.classList.add("is-invalid")
    }
    validateCardHolderName = () => {
        const target = this.element.nativeElement.querySelector("[data-cc-info] input");
        const re = /^([A-Za-z]{3,})\s([A-Za-z]{3,})$/;
        const isValid = re.test(target.value);
        this.flagIfInvalid(target, isValid);
        this.trackcardholder = isValid;
        return isValid
    }
    expiryDateFormatIsValid = (field) => {
        if (/^(([0-9])|((0)[0-9])|((1)[0-3]))(\/)\d{2}$/.test(field.value)) {
            return true;
        }
        return false;
        
    }
   
  
    mastercard: string = "src/src/assets/img/MasterCard_Logo.svg.png"
    visa: string = "../assets/visa-logo.png"
    validateWithLuhn = (digits) => {
        let value = digits;
        if (/[^0-9-\s]+/.test(value)) return false;
        let nCheck = 0, nDigits = 0, bEven = false;
        value = value.replace(/\D/g, '');
        for (let n = value.length - 1; n >= 0; n--) {
            const cDigit = value.charAt(n);
            let nDigits = parseInt(cDigit, 10);
            if (bEven) {
                if ((nDigits *= 2) > 9)
                    nDigits -= 9;
            }
            nCheck += nDigits;
            bEven = !bEven;
        }
        if ((nCheck % 10) == 0) this.cardnumber = digits;
        return (nCheck % 10) == 0
    };
    validateCardNumber = (cardInputs) => {
        const creditCardField = this.element.nativeElement.querySelector("[data-cc-digits]")
        const isValid = this.validateWithLuhn(cardInputs);
     
        if (isValid) {
            this.renderer.removeClass(creditCardField, 'is-invalid');
        }
        else {
            this.renderer.addClass(creditCardField, 'is-invalid')
        }
        this.trackcardvalidation = isValid;
        return isValid
    };
    detectCardType = (first4Digits) => {
        const creditCard = this.element.nativeElement.querySelector("[data-credit-card]")
        const cardTypeField = this.element.nativeElement.querySelector("[data-card-type]")
        const firstDigit = first4Digits[0];
        const cardType = firstDigit == 4 ? 'is-visa' : first4Digits.startsWith("53") ||  first4Digits.startsWith("55") ? 'is-mastercard' : '';
           
         
        if (cardType === 'is-visa') {
            
            this.renderer.addClass(creditCard, 'is-visa')
            this.renderer.removeClass(creditCard, 'is-mastercard')
            cardTypeField.src = "/assets/img/visa-logo.png";
        }
        else if (cardType === 'is-mastercard') {
            this.renderer.addClass(creditCard, 'is-mastercard')
            this.renderer.removeClass(creditCard, 'is-visa')
    
            cardTypeField.src = "assets/img/MasterCard.png";
        }
        else {
            this.renderer.removeClass(creditCard, 'is-visa')
            this.renderer.removeClass(creditCard, 'is-mastercard')
            cardTypeField.src = 'https://placehold.it/120x60.png?text=card';
        }
        return cardType;
    };
    validateCardExpiryDate = () => {
        const presentDate = new Date();
        const dateField = this.element.nativeElement.querySelector('[data-cc-info] input:last-child');
      
        let isValid = this.expiryDateFormatIsValid(dateField);
        if (isValid == true) {
            const cardMonth = Number(dateField.value.split('/')[0]) - 1;
            const cardYear = Number(`20${dateField.value.split('/')[1]}`);
            const cardDate = new Date(cardYear, cardMonth)
            isValid = cardDate > presentDate;
            this.expiry = dateField.value
            console.log(this.expiry)
        }
      
        this.flagIfInvalid(dateField, isValid)
        if (dateField.value.length == dateField.size -1 ) {
            if (this.trackcardholder && this.trackcardvalidation && isValid) {
                console.log("validation successful")
                var val = this.element.nativeElement.querySelector(".pin-element")
                this.renderer.removeClass(val, "d-none")
                console.log(this.cardnumber)
                
            }
            else {
                this. errorMessage = "verification failed"
                console.log("verification failed")
            }
         }
        return isValid;
    }
    smartCursor = () => {
        var fields = this.element.nativeElement.querySelectorAll("input")
        for (var i = 0; i < fields.length; i++){
            if (fields[i].value.length == fields[i].size) {
                fields[i + 1].focus(); 
            } else {
                return;
            }
         
             
        }
       
    }
    
}