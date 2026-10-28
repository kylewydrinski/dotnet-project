# dotStudy

Study Guide Site: Somewhere users can create study terms, as well as pick from a list of premade terms; software terms only.

## Running the Project

### Development

`dotnet watch run`

## Pages

### Home Page

The home page features a simple onboarding note.

### Explore

Where users can find premade study guides and terms.

### My Guides

Where users can find their saved study guides, as well as study guides and terms that they created.

### About

Where users can read about this application, as well as articles written by the creator.

### Log In

Where users can log in to an existing account.

### Sign Up

Where new users can create an account.

## Database Schema

The database is currently run locally using SQL Server Management Studio. The tables are as follows:

### Users

Contains the columns:
- UserId: int
- Username: string
- Email: string
- PasswordHash: string
- Role: string
- CreatedAt: DateTime
And properties:
- Categories: List<Category>
- Terms: List<Term>
- StudyGuides: List<StudyGuide>

### Terms

Contains the columns:
- TermId: int
- TermName: string
- TermDescription: string
- CategoryId: int
- Category: Category
- UserId: int
- User: User
- CreatedAt: DateTime
And property:
- StudyGuideTerms: List<StudyGuideTerm>

### Categories

Categories that terms may fall under. Contains the columns:
- CategoryId: int
- CategoryName: string
- UserId: int
- CreatedAt: DateTime
And properties:
- User: User
- Terms: List<Term>

### StudyGuides

Contains the columns:
- StudyGuideId: int
- Title: string
- Description: string
- UserId: int
- CreatedAt: DateTime
And properties:
- User: User
- StudyGuideTerms: List<StudyGuideTerm>

### StudyGuideTerms

Contains the columns:
- StudyGuideId: int
- TermId: int
- SortOrder: int
And properties:
- StudyGuide: StudyGuide
- Term: Term
