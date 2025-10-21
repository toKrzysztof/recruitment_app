import { Injectable } from '@angular/core';
import { environment } from '../../../../../environment/environment';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContactDto } from '../models/contact-dto';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl = `${environment.apiUrl}/contacts`;

  constructor(private http: HttpClient) {}

  getContacts(page: number, pageSize: number): Observable<HttpResponse<ContactDto[]>> {
    return this.http.get<ContactDto[]>(
      `${this.apiUrl}?pageNumber=${page}&pageSize=${pageSize}`,
      { observe: 'response' }
    );
  }

  getContact(id: number): Observable<ContactDto> {
    return this.http.get<ContactDto>(`${this.apiUrl}/${id}`);
  }

  createContact(contact: ContactDto): Observable<ContactDto> {
    return this.http.post<ContactDto>(this.apiUrl, contact);
  }

  updateContact(contact: ContactDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${contact.id}`, contact);
  }

  deleteContact(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
