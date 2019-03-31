export default{
    methods:{
        validate () {
          return this.$refs.form.validate();
        },
        reset () {
          this.$refs.form.reset()
        },
        resetValidation () {
          this.$refs.form.resetValidation()
        }
    }
}