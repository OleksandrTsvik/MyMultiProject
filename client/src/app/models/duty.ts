export interface Duty {
    id: string;
    position: number;
    title: string;
    description: string;
    isCompleted: boolean;
    dateCreation: string;
    dateCompletion: string | null;
    backgroundColor: string;
    fontColor: string;
  }
  