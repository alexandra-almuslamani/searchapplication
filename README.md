# Hadith Library Fuzzy Search 

A **130k+ Hadith search engine** built with ASP.NET Core MVC.  

## 🏷 Features
- Fuzzy search using RapidFuzz to match similar words and tolerate minor text differences.
- Supports multi-word queries and returns Hadiths containing all searched terms  
- Arabic text normalization by removing diacritics (Tashkeel) and punctuation to improve search accuracy.  
- Search results ranked by match accuracy and sequential word matching.  
- Highlights matched words for easier reading.  
- Simple and user-friendly interface with a search bar and results table.  

## 🔧 Tech
- ASP.NET Core MVC + MS Access database.
- RapidFuzz fuzzy matching algorithm
- Database-first approach using scaffolded models from the existing database.
- Arabic text preprocessing and normalization.
- Designed to handle large datasets (130k+ Hadith records)
<hr>

### 👇 The Interface before searching:
<br>
<img width="1363" height="650" alt="image" src="https://github.com/user-attachments/assets/bb836253-6918-4ff7-8167-bd466aec4c3f" />
<hr>

### 👇 The interface shows results after searching:
<br>
<img width="1366" height="648" alt="image" src="https://github.com/user-attachments/assets/dd6fa804-3175-480f-b69f-c941ab021509" />
