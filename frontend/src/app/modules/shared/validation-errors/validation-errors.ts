import { Component, computed, input } from '@angular/core';
import { FormErrors } from '../models/form-errors';

@Component({
  selector: 'app-validation-errors',
  imports: [],
  templateUrl: './validation-errors.html',
  styleUrl: './validation-errors.scss'
})
export class ValidationErrorsComponent {
  public errors = input.required<FormErrors | null>();
  public touched = input.required<boolean>();
  public dirty = input.required<boolean>();

  private markedForCheck = computed(() => {
    return this.touched() && this.dirty() && this.errors() !== null;
  });

  private requiredError = computed(() => this.errors()?.required);
  private emailFormatError = computed(() => this.errors()?.email);
  private emailTakenError = computed(() => this.errors()?.emailTaken);
  private maxlengthError = computed(() => this.errors()?.maxlength);
  private minLengthError = computed(() => this.errors()?.minlength);
  private minValueError = computed(() => this.errors()?.min);
  private maxValueError = computed(() => this.errors()?.max);
  private numberRequired = computed(() => this.errors()?.numberRequired);
  private specialCharRequired = computed(() => this.errors()?.specialCharRequired);

  protected state = {
    markedForCheck: this.markedForCheck,
    requiredError: this.requiredError,
    emailFormatError: this.emailFormatError,
    emailTakenError: this.emailTakenError,
    maxlengthError: this.maxlengthError,
    minLengthError: this.minLengthError,
    minValueError: this.minValueError,
    maxValueError: this.maxValueError,
    numberRequired: this.numberRequired,
    specialCharRequired: this.specialCharRequired
  };
}
