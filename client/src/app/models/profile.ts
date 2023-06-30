export interface Profile {
    userName: string;
    email: string;
    registrationDate: Date;
    image?: string;
    photos: Photo[];
}

export interface Photo {
    id: string;
    url: string;
    isMain: boolean;
}