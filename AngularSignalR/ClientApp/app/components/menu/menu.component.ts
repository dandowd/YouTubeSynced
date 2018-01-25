
export interface Menu {
    menuItems: MenuItem[];
    isVisible: boolean;
    toggleMenu(username: string, x: number, y: number): void;
}

export interface MenuItem {
    text: string;
}