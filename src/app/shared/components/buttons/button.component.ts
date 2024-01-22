import { Component, Input } from '@angular/core'
import { ButtonType } from '../../../core/ui-models/button-types.enum'

@Component({
  selector: 'rw-shared-button',
  templateUrl: './button.component.html',
})
export class SharedButtonComponent {
    
    @Input() displayText!: string
    @Input() buttonType!: ButtonType
    @Input() counter!: number
    @Input() isPullRight!: boolean
    style() {
        return 
    }

    public get ButtonType() {
        return ButtonType
    }
    constructor() {}

}
