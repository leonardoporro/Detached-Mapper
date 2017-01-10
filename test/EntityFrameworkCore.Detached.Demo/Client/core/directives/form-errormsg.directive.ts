import { Directive, ElementRef, Input } from "@angular/core";
import { FormControl } from "@angular/forms";
import { LocalizationService } from "angular2localization";

@Directive({
    selector: "[errormsg]"
})
export class FormErrorMessageDirective {
    private _formControl: FormControl;

    constructor(private element: ElementRef, 
                private localizationService: LocalizationService) {    
    }

    @Input()
    public get errormsg(): FormControl {
        return this._formControl;
    }
    public set errormsg(value: FormControl) {
        this._formControl = value;
        this._formControl.statusChanges.subscribe(this.onStatusChange.bind(this));
    }

    @Input()
    public fieldName: string;

    onStatusChange() {
        if (!(this._formControl.pristine || this._formControl.valid)) {

            for (let errorName in this._formControl.errors) {
                let errorValue = this._formControl.errors[errorName];
                if (errorValue) {
                    let args = {
                        field: this.fieldName
                    };
                    let errorKey = "core.validation." + errorName;
                    let errorText = this.localizationService.translate(errorKey, args);
                    this.element.nativeElement.innerHTML = errorText;
                }
            }
        } else {
            this.element.nativeElement.innerHTML = null;
        }
    }
}