import { inject, Injectable } from '@angular/core';
import { CategoryDto } from '../models/category-dto';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environment/environment';
import { SubcategoryDto } from '../models/subcategory-dto';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = `${environment.apiUrl}/categories`;
  private http = inject(HttpClient);

  getCategories(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(`${this.apiUrl}`);
  }

  getSubcategories(id: number): Observable<SubcategoryDto[]> {
    return this.http.get<SubcategoryDto[]>(`${this.apiUrl}/${id}/subcategories`);
  }
}
