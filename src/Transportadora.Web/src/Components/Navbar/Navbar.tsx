import { Menu } from "lucide-react";

interface NavbarProps {
  onMenuClick: () => void;
}

export function Navbar({ onMenuClick }: NavbarProps) {
  return (
    <header className="flex items-center justify-between h-16 px-6 bg-blue-900 text-white shadow">
      <div className="flex items-center gap-3">
        <button className="lg:hidden" onClick={onMenuClick}>
          <Menu size={24} />
        </button>

        <span className="text-xl font-bold tracking-wide">
          Route<span className="text-yellow-400">Track</span>
        </span>
      </div>

      <div className="flex items-center gap-4">
        <span className="text-sm opacity-90">Olá, Admin</span>

        <div className="w-9 h-9 rounded-full bg-yellow-400 text-blue-900 flex items-center justify-center font-bold">
          A
        </div>
      </div>
    </header>
  );
}
