import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environment/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContactDto } from '../models/contact-dto';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl = `${environment.apiUrl}/contacts`;
  private http = inject(HttpClient);

  getContacts(params: HttpParams): Observable<HttpResponse<ContactDto[]>> {
    return this.http.get<ContactDto[]>(`${this.apiUrl}`, {
      observe: 'response',
      params
    });
  }

  getContact(id: string): Observable<ContactDto> {
    return this.http.get<ContactDto>(`${this.apiUrl}/${id}`);
  }

  createContact(contact: ContactDto): Observable<ContactDto> {
    return this.http.post<ContactDto>(this.apiUrl, contact);
  }

  updateContact(contact: ContactDto, id: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, contact);
  }

  deleteContact(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
