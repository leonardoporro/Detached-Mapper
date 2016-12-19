import {
  Component,
  ChangeDetectorRef,
  ElementRef,
  Input,
  ViewChild
} from "@angular/core";

@Component({
  selector: "mdl-option",
  host: {
    "[class.mdl-option__container]": "true"
  },
  template: require("./option.html")
})
export class MdlOptionComponent {
  @Input("value") public value: any;
  @ViewChild("contentWrapper") contentWrapper: ElementRef;
  public text: any;
  public multiple: boolean = false;
  public selected: boolean = false;
  public onSelect: any = Function.prototype;

  constructor(private changeDetectionRef: ChangeDetectorRef) {}

  public setMultiple(multiple: boolean) {
    this.multiple = multiple;
    this.changeDetectionRef.detectChanges();
  }

  public updateSelected(value: any) {
    if (this.multiple) {
      this.selected = (value.map((v: any) => this.stringifyValue(v)).indexOf(this.stringValue) != -1);
    } else {
      this.selected = this.value == value;
    }
    this.changeDetectionRef.detectChanges();
  }

  ngAfterViewInit() {
    this.text = this.contentWrapper.nativeElement.textContent.trim();
  }

  get stringValue(): string {
    return this.stringifyValue(this.value);
  }

  private stringifyValue(value: any): string {
    switch (typeof value) {
      case "number": return String(value);
      case "object": return JSON.stringify(value);
      default: return (!!value) ? String(value) : "";
    }
  }
}
