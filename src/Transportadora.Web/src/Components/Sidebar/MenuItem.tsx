import React from "react";
import { NavLink } from "react-router-dom";

interface MenuItemProps {
  icon: React.ReactNode;
  label: string;
  to: string;
}

function MenuItem({ icon, label, to }: MenuItemProps) {
  return (
    <NavLink
      to={to}
      className={({ isActive }) =>
        `
        flex items-center gap-3 px-4 py-3 rounded-lg
        transition cursor-pointer
        ${
          isActive
            ? "bg-blue-600 text-white"
            : "text-gray-300 hover:bg-gray-800"
        }
        `
      }
    >
      {icon}
      <span className="text-sm font-medium">{label}</span>
    </NavLink>
  );
}

export default MenuItem;
