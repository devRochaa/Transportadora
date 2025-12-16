import { LayoutDashboard, Truck, MapPin, Users, Settings } from "lucide-react";
import MenuItem from "./MenuItem";

interface SidebarProps {
  isOpen: boolean;
}

export function Sidebar({ isOpen }: SidebarProps) {
  return (
    <aside
      className={`
        fixed lg:static inset-y-0 left-0 z-50
        w-64 bg-gray-900 text-gray-100
        transform transition-transform duration-300
        ${isOpen ? "translate-x-0" : "-translate-x-full"}
        lg:translate-x-0
      `}
    >
      <div className="p-6 text-xl font-bold border-b border-gray-800">
        🚚 RouteTrack
      </div>

      <nav className="p-4 space-y-2">
        <MenuItem
          icon={<LayoutDashboard size={18} />}
          label="Dashboard"
          to="/"
        />
        <MenuItem icon={<Truck size={18} />} label="Rotas" to="/rotas" />
        <MenuItem
          icon={<MapPin size={18} />}
          label="Rastreamento"
          to="/rastreamento"
        />
        <MenuItem
          icon={<Users size={18} />}
          label="Motoristas"
          to="/motoristas"
        />
        <MenuItem
          icon={<Settings size={18} />}
          label="Configurações"
          to="/configuracoes"
        />
      </nav>
    </aside>
  );
}
