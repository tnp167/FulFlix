import React from "react";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Control } from "react-hook-form";
import { FormFieldType } from "@/types/formFieldType";
import "react-phone-number-input/style.css";
import PhoneInput from "react-phone-number-input";
import { E164Number } from "libphonenumber-js/core";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import {
  Select,
  SelectContent,
  SelectValue,
  SelectTrigger,
  SelectItem,
  DelayedSelect,
} from "./ui/select";
import { default as ReactSelect } from "react-select";

import { Checkbox } from "./ui/checkbox";
import { Calendar } from "lucide-react";
const location = [
  { value: "Shoreditch", label: "Shoreditch" },
  { value: "Paddington", label: "Paddington" },
  { value: "Manchester", label: "Manchester" },
  { value: "Birmingham", label: "Birmingham" },
  { value: "Nottingham", label: "Nottingham" },
];
interface CustomProps {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<any>;
  fieldType: FormFieldType;
  name: string;
  label?: string;
  placeholder?: string;
  iconSrc?: string;
  iconAlt?: string;
  disabled?: boolean;
  dateFormat?: string;
  showTimeSelect?: boolean;
  children?: React.ReactNode;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const RenderField = ({
  field,
  props,
  children,
}: {
  field: any;
  props: CustomProps;
  children?: React.ReactNode;
}) => {
  const { fieldType, placeholder } = props;

  switch (fieldType) {
    case FormFieldType.INPUT:
      return (
        <div className="flex rounded-md">
          <FormControl>
            <Input placeholder={placeholder} {...field} className="text-base" />
          </FormControl>
        </div>
      );
    case FormFieldType.PHONE_INPUT:
      return (
        <FormControl>
          <PhoneInput
            defaultCountry="GB"
            placeholder={placeholder}
            international
            withCountryCallingCode
            value={field.value as E164Number | undefined}
            onChange={field.onChange}
            className="bg-transparent w-full border-transparent outline-none overflow-hidden"
          />
        </FormControl>
      );
    case FormFieldType.DATE_PICKER:
      return (
        <div className="flex rounded-md bg-transparent">
          <Calendar className="w-6 h-6 mr-5" />
          <FormControl>
            <DatePicker
              selected={field.value}
              onChange={(date) => field.onChange(date)}
              dateFormat="dd/MM/yyyy"
              wrapperClassName="date-picker"
              className="bg-transparent w-full border-b pb-2 border-white/60 hover:border-white/100 outline-none overflow-hidden focus-visible:border-secondary transition-colors ease-in-out duration-300"
            />
          </FormControl>
        </div>
      );
    case FormFieldType.CHECKBOX:
      return (
        <FormControl>
          <div className="flex items-center gap-4">
            <Checkbox
              id={props.name}
              checked={field.value}
              onCheckedChange={field.onChange}
            />
            <label
              htmlFor={props.name}
              className="cursor-pointer text-sm font-medium text-secondary peer-disabled:cursor-not-allowed peer-disabled:opacity-70 md:leading-none"
            >
              {props.label}
            </label>
          </div>
        </FormControl>
      );
    case FormFieldType.SELECT:
      return (
        <FormControl>
          <Select onValueChange={field.onChange} defaultValue={field.value}>
            <FormControl>
              <SelectTrigger
                className="shad-select-trigger"
                onClick={(e) => {
                  e.stopPropagation();
                  console.log("Trigger");
                }}
              >
                <SelectValue
                  placeholder={placeholder}
                  onClick={(e) => {
                    e.stopPropagation();
                    console.log("Trigger");
                  }}
                />
              </SelectTrigger>
            </FormControl>
            <SelectContent
              className=""
              onClick={(e) => {
                e.stopPropagation();
                console.log("Trigger");
              }}
            >
              {props.children}
            </SelectContent>
          </Select>
        </FormControl>
      );
    case FormFieldType.REACTSELECT:
      return (
        <ReactSelect
          onChange={field.onChange}
          defaultValue={field.value}
          options={location}
          classNamePrefix="select"
          classNames={{
            control: (state) =>
              state.isFocused ? "border-red-600" : "border-grey-300",
          }}
          placeholder="Select your primary fulflix"
        />
      );
    default:
      break;
  }
};

const CustomFormField = (props: CustomProps) => {
  const { control, fieldType, name, label } = props;
  return (
    <FormField
      control={control}
      name={name}
      render={({ field }) => (
        <FormItem className="flex-1">
          {fieldType !== FormFieldType.CHECKBOX && label && (
            <FormLabel className="text-accent">{label}</FormLabel>
          )}
          <RenderField field={field} props={props} />

          <FormMessage className="shad-error" />
        </FormItem>
      )}
    />
  );
};

export default CustomFormField;
