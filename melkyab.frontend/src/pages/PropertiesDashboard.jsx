import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

export default function PropertiesDashboard() {
  const [properties, setProperties] = useState([]);

  useEffect(() => {
    fetch("http://localhost:8000/api/v1/properties")
      .then(res => res.json())
      .then(data => setProperties(data.items || []));
  }, []);

  const handleDelete = async (id) => {
    if (!window.confirm("آیا مطمئنی می‌خوای حذف کنی؟")) return;
    await fetch(`http://localhost:8000/api/v1/properties/${id}`, { method: "DELETE" });
    setProperties(properties.filter(p => p.property.id !== id));
  };

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">مدیریت ملک‌ها</h1>
        <Link
          to="/properties/create"
          className="bg-blue-600 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-700"
        >
          + ملک جدید
        </Link>
      </div>

      <table className="w-full border-collapse border border-gray-200">
        <thead className="bg-gray-100">
          <tr>
            <th className="border p-2">عنوان</th>
            <th className="border p-2">نوع</th>
            <th className="border p-2">قیمت (هر شب)</th>
            <th className="border p-2">فعال</th>
            <th className="border p-2">عملیات</th>
          </tr>
        </thead>
        <tbody>
          {properties.map(({ property }) => (
            <tr key={property.id} className="text-center">
              <td className="border p-2">{property.title}</td>
              <td className="border p-2">{property.type}</td>
              <td className="border p-2">{property.pricePerNight}</td>
              <td className="border p-2">{property.isActive ? "✅" : "❌"}</td>
              <td className="border p-2 flex justify-center gap-2">
                <Link
                  to={`/properties/edit/${property.id}`}
                  className="bg-yellow-500 text-white px-3 py-1 rounded hover:bg-yellow-600"
                >
                  ویرایش
                </Link>
                <button
                  onClick={() => handleDelete(property.id)}
                  className="bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700"
                >
                  حذف
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
