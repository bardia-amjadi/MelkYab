import { useNavigate } from "react-router-dom";
import PropertyForm from "../components/PropertyForm";

export default function CreateProperty() {
  const navigate = useNavigate();

  const handleSubmit = async (data) => {
    await fetch("http://localhost:8000/api/v1/properties", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });
    navigate("/properties");
  };

  return (
    <div className="p-6">
      <h1 className="text-xl font-bold mb-4">ایجاد ملک جدید</h1>
      <PropertyForm onSubmit={handleSubmit} />
    </div>
  );
}
