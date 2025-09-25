import { useState } from "react";

export default function PropertyForm({ initialData = {}, onSubmit }) {
  const [form, setForm] = useState({
    title: initialData.title || "",
    description: initialData.description || "",
    type: initialData.type || "",
    pricePerNight: initialData.pricePerNight || 0,
    bedrooms: initialData.bedrooms || 0,
    bathrooms: initialData.bathrooms || 0,
    isActive: initialData.isActive || false,
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm({ ...form, [name]: type === "checkbox" ? checked : value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(form);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4 max-w-lg">
      <input
        type="text"
        name="title"
        placeholder="عنوان"
        value={form.title}
        onChange={handleChange}
        className="w-full border p-2 rounded"
      />
      <textarea
        name="description"
        placeholder="توضیحات"
        value={form.description}
        onChange={handleChange}
        className="w-full border p-2 rounded"
      />
      <input
        type="text"
        name="type"
        placeholder="نوع"
        value={form.type}
        onChange={handleChange}
        className="w-full border p-2 rounded"
      />
      <input
        type="number"
        name="pricePerNight"
        placeholder="قیمت هر شب"
        value={form.pricePerNight}
        onChange={handleChange}
        className="w-full border p-2 rounded"
      />
      <input
        type="number"
        name="bedrooms"
        placeholder="تعداد اتاق خواب"
        value={form.bedrooms}
        onChange={handleChange}
        className="w-full border p-2 rounded"
      />
      <input
        type="number"
        name="bathrooms"
        placeholder="تعداد سرویس بهداشتی"
        value={form.bathrooms}
        onChange={handleChange}
        className="w-full border p-2 rounded"
      />
      <label className="flex items-center gap-2">
        <input
          type="checkbox"
          name="isActive"
          checked={form.isActive}
          onChange={handleChange}
        />
        فعال باشد
      </label>
      <button
        type="submit"
        className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
      >
        ذخیره
      </button>
    </form>
  );
}
